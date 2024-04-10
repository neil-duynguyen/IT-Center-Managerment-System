using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Requests;
using KidProEdu.Application.ViewModels.RequestUserAccountViewModels;
using KidProEdu.Application.ViewModels.RequestViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics;

namespace KidProEdu.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public RequestService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateRequest(CreateRequestViewModel createRequestViewModel)
        {
            var validator = new CreateRequestViewModelValidator();
            var validationResult = validator.Validate(createRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            if (!createRequestViewModel.RequestType.Equals("Location")
                && !createRequestViewModel.RequestType.Equals("Class")
                && !createRequestViewModel.RequestType.Equals("Equipment")
                && !createRequestViewModel.RequestType.Equals("Schedule")
                && !createRequestViewModel.RequestType.Equals("Refund")
                && !createRequestViewModel.RequestType.Equals("Leave")
                )
                throw new Exception("Loại yêu cầu không có trong hệ thống");

            Request request = new()
            {
                RequestDescription = createRequestViewModel.RequestDescription,
                RequestType = createRequestViewModel.RequestType,
                LeaveDate = createRequestViewModel.LeaveDate,
                TeachingDay = createRequestViewModel.TeachingDate,
                EquimentType = createRequestViewModel.EquimentType,
                LocationId = createRequestViewModel.LocationId,
                FromClassId = createRequestViewModel.FromClassId,
                ToClassId = createRequestViewModel.ToClassId,
                ScheduleId = createRequestViewModel.ScheduleId,
                ReceiverRefundId = createRequestViewModel.ReceiverRefundId,
            };

            await _unitOfWork.RequestRepository.AddAsync(request);

            //tạo request user account
            CreateRequestUserAccountViewModel model = new()
            {
                RequestId = request.Id,
                RecieverIds = createRequestViewModel.UserIds,
                Status = Domain.Enums.StatusOfRequest.Pending
            };
            RequestUserAccountService service = new(_unitOfWork, _currentTime, _claimsService, _mapper);
            await service.CreateRequestUserAccount(model);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo yêu cầu thất bại");

        }

        public async Task<bool> DeleteRequest(Guid requestId)
        {
            var result = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);

            if (result == null)
                throw new Exception("Không tìm thấy yêu cầu này");
            else
            {
                _unitOfWork.RequestRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa yêu cầu thất bại");
            }
        }

        public async Task<Request> GetRequestById(Guid RequestId)
        {
            var request = await _unitOfWork.RequestRepository.GetByIdAsync(RequestId);
            return request;
        }

        public async Task<List<RequestViewModel>> GetRequests()
        {
            var requests = _unitOfWork.RequestRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<RequestViewModel>>(requests);
        }

        public async Task<bool> UpdateRequest(UpdateRequestViewModel updateRequestViewModel)
        {
            var validator = new UpdateRequestViewModelValidator();
            var validationResult = validator.Validate(updateRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var request = await _unitOfWork.RequestRepository.GetByIdAsync(updateRequestViewModel.Id)
                ?? throw new Exception("Không tìm thấy yêu cầu");
            var requestUser = _unitOfWork.RequestUserAccountRepository.GetAllAsync().Result
                .FirstOrDefault(x => x.RequestId == request.Id && x.IsDeleted == false
                && x.Status != Domain.Enums.StatusOfRequest.Pending);

            /*var existingRequest = await _unitOfWork.RequestRepository.GetRequestByRequestType(updateRequestViewModel.RequestType);
            if (!existingRequest.IsNullOrEmpty())
            {
                if (existingRequest.FirstOrDefault().Id != updateRequestViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }*/

            if (requestUser.Status != null)
                throw new Exception("Cập nhật yêu cầu thất bại, yêu cầu này đã được xử lý");

            /*Request.RequestName = updateRequestViewModel.RequestName;
            Request.Description = updateRequestViewModel.Description;*/

            var mapper = _mapper.Map(updateRequestViewModel, request);

            _unitOfWork.RequestRepository.Update(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật yêu cầu thất bại");
        }

        public async Task<bool> ChangeStatusRequest(ChangeStatusRequestViewModel changeStatusRequestViewModel)
        {
            var status = Domain.Enums.StatusOfRequest.Pending;

            switch (changeStatusRequestViewModel.Status)
            {
                case "Approved":
                    status = Domain.Enums.StatusOfRequest.Approved;

                    foreach (var idRequest in changeStatusRequestViewModel.RequestIds)
                    {
                        var request = await _unitOfWork.RequestRepository.GetByIdAsync(idRequest);
                        var requestUsers = await _unitOfWork.RequestUserAccountRepository.GetRequestUserByRequestId(idRequest);
                        switch (request.RequestType)
                        {
                            case "Location":
                                var user = await _unitOfWork.UserRepository.GetByIdAsync((Guid)request.CreatedBy);
                                user.LocationId = changeStatusRequestViewModel.LocationId;
                                _unitOfWork.UserRepository.Update(user);
                                break;
                            case "Class":
                                var teacher = request.CreatedBy;
                                var currentTeaching = _unitOfWork.TeachingClassHistoryRepository
                                    .GetTeachingHistoryByStatus(Domain.Enums.TeachingStatus.Pending).Result
                                    .FirstOrDefault(x => x.ClassId == request.FromClassId);
                                var newTeaching = _unitOfWork.TeachingClassHistoryRepository
                                    .GetTeachingHistoryByStatus(Domain.Enums.TeachingStatus.Pending).Result
                                    .FirstOrDefault(x => x.ClassId == request.ToClassId);
                                if (currentTeaching != null && newTeaching != null)
                                {
                                    //đổi lớp ở đây là chưa bắt đầu, còn khi đã bắt đầu r mà gv khác vô dạy thì thêm 
                                    //record history r đổi status cũ
                                    currentTeaching.UserAccountId = newTeaching.UserAccountId; //lớp cũ đổi thành gv lớp mới
                                    newTeaching.UserAccountId = (Guid)teacher; //lớp mới đổi thành giáo viên cũ
                                }
                                else
                                {
                                    throw new Exception("Không thể thực hiện đổi khi đã có lớp không còn ở trạng thái chờ");
                                }

                                _unitOfWork.TeachingClassHistoryRepository.Update(currentTeaching);
                                _unitOfWork.TeachingClassHistoryRepository.Update(newTeaching);
                                break;
                            case "Schedule":
                                // đổi lịch thì update endDate và status leaved cho record TeachingHistory cũ và
                                // add 1 record mới cho gv mới sau khi add xong mới thêm 1 record lại cho gv cũ với
                                // trạng thái teaching
                                var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync((Guid)request.ScheduleId);

                                // lấy lịch còn lại của lớp đó
                                var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(schedule.Id);
                                var otherSchedule = findClass.Schedules.FirstOrDefault(x => x.Id != schedule.Id);


                                var tc = request.CreatedBy; // lấy gv, là người gửi request
                                var currentHistory = _unitOfWork.TeachingClassHistoryRepository
                                    .GetTeachingHistoryByStatus(Domain.Enums.TeachingStatus.Teaching).Result
                                    .FirstOrDefault(x => x.ClassId == schedule.ClassId);
                                if (currentHistory != null)
                                {
                                    for (var i = 0; i < 7; i++)
                                    {
                                        // ngày tương ứng lịch còn lại trong quá khứ và tương lai
                                        var getDateBefore = changeStatusRequestViewModel.TeachingDate.Value.AddDays(-i);
                                        var getDateAfter = changeStatusRequestViewModel.TeachingDate.Value.AddDays(7 - i);

                                        if (getDateBefore.DayOfWeek.ToString() == otherSchedule.DayInWeek)
                                        {
                                            currentHistory.EndDate = getDateBefore;
                                            //currentHistory.TeachingStatus = Domain.Enums.TeachingStatus.Leaved

                                            var continueHistory = new TeachingClassHistory
                                            {
                                                ClassId = currentHistory.ClassId,
                                                UserAccountId = currentHistory.UserAccountId,
                                                StartDate = getDateAfter,
                                                TeachingStatus = Domain.Enums.TeachingStatus.Teaching
                                            };

                                            _unitOfWork.TeachingClassHistoryRepository.Update(currentHistory);
                                            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(continueHistory);
                                        }

                                        var getDate = changeStatusRequestViewModel.TeachingDate.Value.AddDays(i);
                                        if (getDate.DayOfWeek.ToString() == schedule.DayInWeek)
                                        {

                                            var newHistory = new TeachingClassHistory
                                            {
                                                ClassId = currentHistory.ClassId,
                                                UserAccountId = requestUsers.FirstOrDefault(x => x.Status == Domain.Enums.StatusOfRequest.Pending).RecieverId,
                                                StartDate = (DateTime)changeStatusRequestViewModel.TeachingDate,
                                                EndDate = (DateTime)changeStatusRequestViewModel.TeachingDate,
                                                TeachingStatus = Domain.Enums.TeachingStatus.Substitute
                                            };

                                            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(newHistory);
                                        }
                                    }
                                    /*if (schedule.DayInWeek.ToLower().Equals("thursday") // 5 8
                                        || schedule.DayInWeek.ToLower().Equals("tuesday") // 3 6
                                        || schedule.DayInWeek.ToLower().Equals("wednesday")) // 4 7
                                    {

                                        currentHistory.EndDate = changeStatusRequestViewModel.TeachingDate.Value.AddDays(-4);
                                        //currentHistory.TeachingStatus = Domain.Enums.TeachingStatus.Leaved;

                                        var newHistory = new TeachingClassHistory
                                        {
                                            ClassId = currentHistory.ClassId,
                                            UserAccountId = requestUsers.FirstOrDefault(x => x.Status == Domain.Enums.StatusOfRequest.Pending).RecieverId,
                                            StartDate = (DateTime)changeStatusRequestViewModel.TeachingDate,
                                            EndDate = (DateTime)changeStatusRequestViewModel.TeachingDate,
                                            TeachingStatus = Domain.Enums.TeachingStatus.Substitute
                                        };

                                        var continueHistory = new TeachingClassHistory
                                        {
                                            ClassId = currentHistory.ClassId,
                                            UserAccountId = currentHistory.UserAccountId,
                                            StartDate = changeStatusRequestViewModel.TeachingDate.Value.AddDays(3),
                                            TeachingStatus = Domain.Enums.TeachingStatus.Teaching
                                        };

                                        _unitOfWork.TeachingClassHistoryRepository.Update(currentHistory);
                                        await _unitOfWork.TeachingClassHistoryRepository.AddAsync(newHistory);
                                        await _unitOfWork.TeachingClassHistoryRepository.AddAsync(continueHistory);

                                    }
                                    else
                                    {
                                        currentHistory.EndDate = changeStatusRequestViewModel.TeachingDate.Value.AddDays(-3);
                                        //currentHistory.TeachingStatus = Domain.Enums.TeachingStatus.Leaved;

                                        var newHistory = new TeachingClassHistory
                                        {
                                            ClassId = currentHistory.ClassId,
                                            UserAccountId = requestUsers.FirstOrDefault(x => x.Status == Domain.Enums.StatusOfRequest.Pending).RecieverId,
                                            StartDate = (DateTime)changeStatusRequestViewModel.TeachingDate,
                                            EndDate = (DateTime)changeStatusRequestViewModel.TeachingDate,
                                            TeachingStatus = Domain.Enums.TeachingStatus.Substitute
                                        };

                                        var continueHistory = new TeachingClassHistory
                                        {
                                            ClassId = currentHistory.ClassId,
                                            UserAccountId = currentHistory.UserAccountId,
                                            StartDate = changeStatusRequestViewModel.TeachingDate.Value.AddDays(4),
                                            TeachingStatus = Domain.Enums.TeachingStatus.Teaching
                                        };

                                        _unitOfWork.TeachingClassHistoryRepository.Update(currentHistory);
                                        await _unitOfWork.TeachingClassHistoryRepository.AddAsync(newHistory);
                                        await _unitOfWork.TeachingClassHistoryRepository.AddAsync(continueHistory);

                                    }*/
                                }
                                else
                                {
                                    throw new Exception("Không tìm thấy lịch này");
                                }
                                break;
                            case "Equipment": //khi approved quét mã cho mượn thì cập nhật lại trạng thái phía dưới
                                break;
                            case "Leave": //approved thì nghỉ
                                          // nghỉ này là nghỉ luôn, nên nghỉ thì disable hoặc xóa account 

                                var teacherAccount = await _unitOfWork.UserRepository.GetByIdAsync((Guid)request.CreatedBy);
                                teacherAccount.Status = Domain.Enums.StatusUser.Disable;

                                _unitOfWork.UserRepository.Update(teacherAccount);

                                break;
                            case "Refund": //approved thì refund 
                                break;
                            default:
                                //throw new Exception("Loại request không được hỗ trợ");
                                break;
                        }

                        foreach (var item in requestUsers)
                        {
                            item.Status = status;
                            _unitOfWork.RequestUserAccountRepository.Update(item);
                        }

                    }

                    break;
                /*case "Pending":
                    status = Domain.Enums.StatusOfRequest.Pending;
                    break;*/
                case "Cancel":
                    status = Domain.Enums.StatusOfRequest.Cancel;
                    foreach (var idRequest in changeStatusRequestViewModel.RequestIds)
                    {
                        var requestUsers = await _unitOfWork.RequestUserAccountRepository.GetRequestUserByRequestId(idRequest);
                        foreach (var item in requestUsers)
                        {
                            item.Status = status;
                            _unitOfWork.RequestUserAccountRepository.Update(item);
                        }
                    }

                    break;
                default:
                    throw new Exception("Trạng thái không được hỗ trợ");
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trạng thái yêu cầu thất bại");
        }

        public async Task<List<RequestViewModel>> GetRequestByUser(Guid userId)
        {
            var requests = await _unitOfWork.RequestRepository.GetRequestByUser(userId);

            return _mapper.Map<List<RequestViewModel>>(requests);
        }

        public async Task<List<RequestViewModel>> GetRequestByReceiver(Guid userId)
        {
            var listRequest = new List<RequestViewModel>();
            var requestUsers = await _unitOfWork.RequestUserAccountRepository.GetRequestUserByRecieverId(userId);
            foreach (var item in requestUsers)
            {
                var request = await _unitOfWork.RequestRepository.GetByIdAsync(item.RequestId);
                listRequest.Add(_mapper.Map<RequestViewModel>(request));
            }

            return _mapper.Map<List<RequestViewModel>>(listRequest);
        }
    }
}
