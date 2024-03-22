using AutoMapper;
using AutoMapper.Execution;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.AdviseRequests;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

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
            var validator = new CreateAdviseRequestViewModelValidator();
            var validationResult = validator.Validate(createAdviseRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
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
            
            var adviseRequests = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByTestDate(createAdviseRequestViewModel.TestDate);
            if (adviseRequests != null && adviseRequests.Count == 5)
            {
                throw new Exception("Lịch đánh giá cho thời gian này đã đủ số lượng");
            }

            var mapper = _mapper.Map<AdviseRequest>(createAdviseRequestViewModel);
            mapper.StatusAdviseRequest = Domain.Enums.StatusAdviseRequest.Pending;

            await _unitOfWork.AdviseRequestRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo yêu cầu tư vấn thất bại");
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
            var mapper = _mapper.Map<List<AdviseRequestViewModel>>(results);
            return mapper;
        }

        public async Task<List<AdviseRequestViewModel>> GetAdviseRequestByTestDate(DateTime testDate)
        {
            var results = _unitOfWork.AdviseRequestRepository.GetAllAsync().Result
                .Where(x => x.IsDeleted == false && x.TestDate.Date == testDate.Date)
                .OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<AdviseRequestViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateAdviseRequest(UpdateAdviseRequestViewModel updateAdviseRequestViewModel, params Expression<Func<AdviseRequest, object>>[] uniqueProperties)
        {
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

            if (adviseRequest.UserId != null)
            {
                if (updateAdviseRequestViewModel.UserId != adviseRequest.UserId)
                {

                    throw new Exception("Yêu cầu tư vấn này đã có người nhận");
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
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật yêu cầu tư vấn thất bại");
        }

        public async Task AutoSendEmail()
        {
            /*var advises = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByTestDate(_currentTime.GetCurrentTime().AddDays(1));
            foreach (var item in advises)
            {*/
                await SendEmailUtil.SendEmail("tkchoi1312@gmail.com", "Nhắc nhở ngày tham gia làm bài kiểm tra đầu vào của KidPro", 
                    "hệ thống kidpro kính gửi thông báo đến quý phụ huynh và học viên có lịch hẹn sẽ làm bài kiểm tra đầu vào của kidpro vào lúc 7h ngày 23, chúc bạn 1 ngày tốt lành  " +
                    "\"<div style=\\\"font-weight: bold;\\\">Trân trọng, <br>\\r\\n        <div style=\\\"color: #FF630E;\\\">Bộ phận hỗ trợ học viên BSMART</div>\\r\\n    </div>\\r\\n<br>    <img src=\\\"cid:image1\\\" alt=\\\"\\\" width=\\\"200px\\\">\\r\\n    <br>\\r\\n    <br>\\r\\n    <div>\\r\\n        Email liên hệ: admin@bsmart.edu.vn\\r\\n    </div>\\r\\n    <div>Số điện thoại: 028 9999 79 39</div>\\r\\n</div>\";");
            //}
        }
    }
}
