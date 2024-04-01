using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Children;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ChildrenService : IChildrenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly IChildrenAnswerService _childrenAnswerService;

        public ChildrenService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, IChildrenAnswerService childrenAnswerService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _childrenAnswerService = childrenAnswerService;
        }

        public async Task<bool> CreateChildren(CreateChildrenViewModel createChildrenViewModel)
        {
            var validator = new CreateChildrenViewModelValidator();
            var validatorResult = await validator.ValidateAsync(createChildrenViewModel);
            if (!validatorResult.IsValid)
            {
                foreach (var error in validatorResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var mapper = _mapper.Map<ChildrenProfile>(createChildrenViewModel);
            var randomCode = StringUtils.GenerateRandomString(2, 6);
            while (_unitOfWork.ChildrenRepository.GetAllAsync().Result.FirstOrDefault(x => x.ChildrenCode == randomCode) != null)
            {
                randomCode = StringUtils.GenerateRandomString(2, 6);
            }

            mapper.ChildrenCode = randomCode;

            await _unitOfWork.ChildrenRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo trẻ thất bại.");
        }

        public async Task<bool> UpdateChildren(UpdateChildrenViewModel updateChildrenViewModel)
        {
            var validator = new UpdateChildrenViewModelValidator();
            var validatorResult = await validator.ValidateAsync(updateChildrenViewModel);
            if (!validatorResult.IsValid)
            {
                foreach (var error in validatorResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var mapper = _mapper.Map<ChildrenProfile>(updateChildrenViewModel);
            _unitOfWork.ChildrenRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trẻ thất bại.");
        }

        public Task<bool> DeleteChildren(Guid childrenId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChildrenViewModel>> GetChildrensByStaffId()
        {
            var getChildrens = _unitOfWork.ChildrenRepository.GetAllAsync().Result.Where(x => x.CreatedBy == _claimsService.GetCurrentUserId).ToList();
            return _mapper.Map<List<ChildrenViewModel>>(getChildrens);
        }

        public async Task<ChildrenViewModel> GetChildrenById(Guid childrenId)
        {
            var result = await _unitOfWork.ChildrenRepository.GetByIdAsync(childrenId);

            var mapper = _mapper.Map<ChildrenViewModel>(result);

            List<ClassViewModelInChildren> listClass = new List<ClassViewModelInChildren>();
            List<CourseViewModelInChildren> listCourse = new List<CourseViewModelInChildren>();
            List<ExamViewModelInChildren> listExam = new List<ExamViewModelInChildren>();

            foreach (var enrollment in result.Enrollments)
            {
                listClass.Add(new ClassViewModelInChildren() { ClassId = enrollment.Class.Id, ClassCode = enrollment.Class.ClassCode });
                mapper.Classes = listClass;
            }

            foreach (var course in result.Enrollments)
            {
                listCourse.Add(new CourseViewModelInChildren() { CourseId = course.Class.Course.Id, CourseCode = course.Class.Course.CourseCode });
                mapper.Courses = listCourse;
            }

            listExam = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                 .Where(x => x.ChildrenProfileId == childrenId)
                 .GroupBy(x => x.ExamId)
                 .Select(group => new ExamViewModelInChildren
                 {
                     ExamName = group.FirstOrDefault()?.Exam.TestName,
                     ExamDate = group.FirstOrDefault()?.Exam.CreationDate,
                     Score = group.Sum(x => x.ScorePerQuestion)
                 })
                 .ToList();

            return mapper;

        }

        public async Task<List<ChildrenViewModel>> GetChildrenByParentId(Guid Id)
        {
            var getChildrens = _unitOfWork.ChildrenRepository.GetAllAsync().Result.Where(x => x.UserId == Id).ToList();
            return _mapper.Map<List<ChildrenViewModel>>(getChildrens);
        }

        public async Task<bool> ChildrenReserve(ChildrenReserveViewModel childrenReserveViewModel)
        {
            var validator = new ChildrenReserveViewModelValidator();
            var validatorResult = await validator.ValidateAsync(childrenReserveViewModel);
            if (!validatorResult.IsValid)
            {
                foreach (var error in validatorResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var children = await _unitOfWork.ChildrenRepository.GetByIdAsync(childrenReserveViewModel.ChildrenProfileId);
            var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(childrenReserveViewModel.ClassId);
            if(children == null)
            {
                throw new Exception("Không tìm thấy học sinh này");
            }

            if (children.Status == StatusChildrenProfile.Reserve)
            {
                throw new Exception("Học sinh này đã bảo lưu rồi");
            }


            foreach (var schedule in schedules)
            {
                var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilIdAndStatusFuture(schedule.Id, childrenReserveViewModel.ChildrenProfileId);
                _unitOfWork.AttendanceRepository.RemoveRange(attendances);
                await _unitOfWork.SaveChangeAsync();
            }

            children.Status = StatusChildrenProfile.Reserve;
            _unitOfWork.ChildrenRepository.Update(children);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bảo lưu thất bại");

        }
    }
}
