using AutoMapper;
using AutoMapper.Execution;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.AdviseRequests;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace KidProEdu.Application.Services
{
    public class AdviseRequestService : IAdviseRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public AdviseRequestService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        public async Task<bool> CreateAdviseRequest(CreateAdviseRequestViewModel createAdviseRequestViewModel)
        {
            var validator = new CreateAdviseRequestViewModelValidator(_currentTime);
            var validationResult = validator.Validate(createAdviseRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            if (createAdviseRequestViewModel.TestDate.Value.Date <= _currentTime.GetCurrentTime().Date &&
                createAdviseRequestViewModel.EndTime < _currentTime.GetCurrentTime())
            {
                throw new Exception("Thời gian đăng kí tư vấn đã qua");
            }

            var adviseRequest = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByEmail(createAdviseRequestViewModel.Email);
            if (adviseRequest != null)
            {
                throw new Exception("Email đã tồn tại");
            }

            adviseRequest = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByPhone(createAdviseRequestViewModel.Phone);
            if (adviseRequest != null)
            {
                throw new Exception("Số điện thoại đã tồn tại");
            }

            if (createAdviseRequestViewModel.TestDate != null && createAdviseRequestViewModel.StartTime != null)
            {
                var adviseRequests = _unitOfWork.AdviseRequestRepository.GetAllAsync().Result.Where(x => x.IsTested == false
               && x.TestDate.Date.Equals(createAdviseRequestViewModel.TestDate.Value.Date)
               && x.SlotId == createAdviseRequestViewModel.SlotId
               && x.IsTested == false).ToList();
                if (adviseRequests != null && adviseRequests.Count >= 5)
                {
                    throw new Exception("Lịch đánh giá cho thời gian này đã đủ số lượng");
                }
            }

            var mapper = _mapper.Map<AdviseRequest>(createAdviseRequestViewModel);
            mapper.StatusAdviseRequest = Domain.Enums.StatusAdviseRequest.Pending;

            await _unitOfWork.AdviseRequestRepository.AddAsync(mapper);

            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                if (createAdviseRequestViewModel.TestDate == null && createAdviseRequestViewModel.StartTime == null
                    && createAdviseRequestViewModel.SlotId == null && createAdviseRequestViewModel.EndTime == null)
                {
                    await SendEmailUtil.SendEmail(mapper.Email, "Xác nhận yêu cầu tư vấn",
                            "<html><body>" +
                            "<p>Kính gửi quý phụ huynh,</p>" +
                            "<p>Yêu cầu tư vấn của bạn đã được xác nhận,</p>" +
                            "<p>Thông tin:</p>" +
                            "<ul>" +
                                "<li>Người đăng kí: " + createAdviseRequestViewModel.FullName + "</li>" +
                                "<li>Email: " + createAdviseRequestViewModel.Email + "</li>" +
                                "<li>Số điện thoại: " + createAdviseRequestViewModel.Phone + "</li>" +
                            "</ul>" +
                            "<p>Nhân viên của chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.</p>" +
                            "<p>Trân trọng,</p>" +
                            "<p>KidPro Education!</p>" +
                            "</body></html>");
                }
                else
                {    
                    await SendEmailUtil.SendEmail(mapper.Email, "Xác nhận yêu cầu tư vấn",
                            "<html><body>" +
                            "<p>Kính gửi quý phụ huynh,</p>" +
                            "<p>Yêu cầu tư vấn của bạn đã được xác nhận,</p>" +
                            "<p>Thông tin:</p>" +
                            "<ul>" +
                                "<li>Người đăng kí: " + createAdviseRequestViewModel.FullName + "</li>" +
                                "<li>Email: " + createAdviseRequestViewModel.Email + "</li>" +
                                "<li>Số điện thoại: " + createAdviseRequestViewModel.Phone + "</li>" +
                                "<li>Vào ngày: " + createAdviseRequestViewModel.TestDate + "</li>" +
                                "<li>Từ " + createAdviseRequestViewModel.StartTime.Value.ToShortTimeString() + " đến " + createAdviseRequestViewModel.EndTime.Value.ToShortTimeString() + "</li>" +
                            "</ul>" +
                            "<p>Nhân viên của chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.</p>" +
                            "<p>Trân trọng,</p>" +
                            "<p>KidPro Education!</p>" +
                            "</body></html>");
                }
                return true;
            }
            else
            {
                throw new Exception("Tạo yêu cầu tư vấn thất bại");
            }
        }

        public async Task<bool> DeleteAdviseRequest(Guid AdviseRequestId)
        {
            var result = await _unitOfWork.AdviseRequestRepository.GetByIdAsync(AdviseRequestId);

            if (result == null)
                throw new Exception("Không tìm thấy yêu cầu tư vấn này");
            else
            {
                _unitOfWork.AdviseRequestRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa yêu cầu tư vấn thất bại");
            }
        }

        public async Task<AdviseRequestViewModel> GetAdviseRequestById(Guid AdviseRequestId)
        {
            var result = await _unitOfWork.AdviseRequestRepository.GetByIdAsync(AdviseRequestId);
            var mapper = _mapper.Map<AdviseRequestViewModel>(result);
            return mapper;
        }

        public async Task<List<AdviseRequestViewModel>> GetAdviseRequests()
        {
            var results = _unitOfWork.AdviseRequestRepository.GetAllAsync().Result
                .Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            List<AdviseRequestViewModel> list = new List<AdviseRequestViewModel>();
            foreach (var item in results)
            {
                var mapper = _mapper.Map<AdviseRequestViewModel>(item);
                if (mapper.UserId != null)
                {
                    mapper.Staff = _mapper.Map<UserViewModel>(await _unitOfWork.UserRepository.GetByIdAsync((Guid)item.UserId));
                }
                list.Add(mapper);
            }

            return list;
        }

        public async Task<List<AdviseRequestViewModel>> GetAdviseRequestByTestDate(DateTime testDate)
        {
            var results = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByTestDate(testDate);
            var mapper = _mapper.Map<List<AdviseRequestViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateAdviseRequest(UpdateAdviseRequestViewModel updateAdviseRequestViewModel, params Expression<Func<AdviseRequest, object>>[] uniqueProperties)
        {
            var count = 0;
            var validator = new UpdateAdviseRequestViewModelValidator();
            var validationResult = validator.Validate(updateAdviseRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var adviseRequest = await _unitOfWork.AdviseRequestRepository.GetByIdAsync(updateAdviseRequestViewModel.Id)
                ?? throw new Exception("Không tìm thấy yêu cầu tư vấn");

            /*if (adviseRequest.UserId != null)
            {
                if (updateAdviseRequestViewModel.UserId != adviseRequest.UserId)
                {

                    throw new Exception("Yêu cầu tư vấn này đã có người nhận");
                }
            }*/

            if (updateAdviseRequestViewModel.TestDate != null && updateAdviseRequestViewModel.StartTime != null)
            {
                if (updateAdviseRequestViewModel.TestDate.Value.Date.Equals(adviseRequest.TestDate.Date) &&
                    updateAdviseRequestViewModel.SlotId.Equals(adviseRequest.SlotId) &&
                    updateAdviseRequestViewModel.StartTime.Value.Equals(adviseRequest.StartTime.Value))
                {

                }
                else
                {
                    var adviseRequests = _unitOfWork.AdviseRequestRepository.GetAllAsync().Result.Where(x => x.IsTested == false
                    && x.TestDate.Date.Equals(updateAdviseRequestViewModel.TestDate.Value.Date)
                    && x.SlotId == updateAdviseRequestViewModel.SlotId
                    && x.IsTested == false).ToList();
                    if (adviseRequests != null && adviseRequests.Count >= 5)
                    {
                        throw new Exception("Lịch đánh giá cho thời gian này đã đủ số lượng");
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            foreach (var property in uniqueProperties)
            {
                var obj = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByProperty(updateAdviseRequestViewModel, property);
                if (obj != null && obj.Id != updateAdviseRequestViewModel.Id)
                {
                    throw new Exception($"{property.GetMember().Name} đã tồn tại");
                }
            }

            var mapper = _mapper.Map(updateAdviseRequestViewModel, adviseRequest);

            _unitOfWork.AdviseRequestRepository.Update(mapper);

            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                if (count > 0)
                {
                    await SendEmailUtil.SendEmail(mapper.Email, "Xác nhận thay đổi lịch đăng kí tham gia đánh giá đầu vào",
                            "<html><body>" +
                            "<p>Kính gửi quý phụ huynh,</p>" +
                            "<p>Xác nhận lịch đăng kí tham gia đánh giá đầu vào của trẻ đã thay đổi sang:</p>" +
                            "<p>Thời gian: " + mapper.StartTime + " - " + mapper.EndTime + "</p>" +
                            "<p>Ngày: " + mapper.TestDate + "</p>" +
                            "<p>Trân trọng,</p>" +
                            "<p>KidPro Education!</p>" +
                            "</body></html>");
                }
                return true;
            }
            else
            {
                throw new Exception("Cập nhật yêu cầu tư vấn thất bại");
            }
        }

        public async Task AutoSendEmail()
        {
            var advises = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByTestDate(_currentTime.GetCurrentTime().AddDays(1));
            foreach (var item in advises)
            {
                await SendEmailUtil.SendEmail(item.Email, "Nhắc nhở ngày tham gia làm bài kiểm tra đầu vào của KidPro",
                         "<html><body>" +
                         "<p>Kính gửi quý phụ huynh,</p>" +
                         "<p>KidPro xin gửi thông báo đến quý phụ huynh có trẻ đã đăng kí lịch làm bài kiểm tra đầu vào của KidPro:</p>" +
                         "<p>Thời gian: " + item.StartTime + " - " + item.EndTime + "</p>" +
                         "<p>Ngày: " + item.TestDate.ToString("dd/MM/yyyy") + "</p>" +
                         "<p>Xin cảm ơn,</p>" +
                         "<p>KidPro Education!</p>" +
                         "</body></html>");
            }
        }

        public async Task<List<AdviseRequestViewModel>> GetAdviseRequestByUserId(Guid id)
        {
            var adviseRequests = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByUserId(id);

            List<AdviseRequestViewModel> list = new();
            foreach (var item in adviseRequests)
            {
                var mapper = _mapper.Map<AdviseRequestViewModel>(item);
                if (mapper.UserId != null)
                {
                    mapper.Staff = _mapper.Map<UserViewModel>(await _unitOfWork.UserRepository.GetByIdAsync((Guid)item.UserId));
                }
                list.Add(mapper);
            }

            return _mapper.Map<List<AdviseRequestViewModel>>(list);
        }
    }
}
