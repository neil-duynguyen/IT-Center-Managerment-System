using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Children;
using KidProEdu.Application.ViewModels.CertificateViewModel;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.TransactionViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
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
            var getChildern = await _unitOfWork.ChildrenRepository.GetByIdAsync(updateChildrenViewModel.Id);
            if (getChildern is not null)
            {
                var mapper = _mapper.Map(updateChildrenViewModel, getChildern);
                _unitOfWork.ChildrenRepository.Update(mapper);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trẻ thất bại.");
            }
            else
            {
                throw new Exception("Cập nhật trẻ thất bại.");
            }
        }

        public Task<bool> DeleteChildren(Guid childrenId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChildrenViewModel>> GetChildrensByStaffId()
        {
            var getChildrens = _unitOfWork.ChildrenRepository.GetAllAsync().Result.Where(x => x.CreatedBy == _claimsService.GetCurrentUserId).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<ChildrenViewModel>>(getChildrens);
        }

        public async Task<ChildrenViewModel> GetChildrenById(Guid childrenId)
        {
            var result = await _unitOfWork.ChildrenRepository.GetByIdAsync(childrenId);

            var mapper = _mapper.Map<ChildrenViewModel>(result);

            List<ClassViewModelInChildren> listClass = new List<ClassViewModelInChildren>();
            List<CourseViewModelInChildren> listCourse = new List<CourseViewModelInChildren>();
            List<ExamViewModelInChildren> listExam = new List<ExamViewModelInChildren>();
            List<CertificateViewModel> listCertificate = new List<CertificateViewModel>();

            foreach (var enrollment in result.Enrollments)
            {
                listClass.Add(new ClassViewModelInChildren() { ClassId = enrollment.Class.Id, ClassCode = enrollment.Class.ClassCode, StatusOfClass = enrollment.Class.StatusOfClass.ToString() });
                mapper.Classes = listClass;
            }

            foreach (var course in result.Enrollments)
            {
                listCourse.Add(new CourseViewModelInChildren() { CourseId = course.Class.Course.Id, CourseCode = course.Class.Course.CourseCode });
                mapper.Courses = listCourse;
            }

            foreach (var item in result.Certificates)
            {
                listCertificate.Add(new CertificateViewModel() { ChildrenProfileId = item.ChildrenProfileId, CourseId = item.CourseId, FullName = result.FullName, CourseName = _unitOfWork.CourseRepository.GetByIdAsync(item.CourseId).Result.Name, Code = item.Code, Url = item.Url,  CreateDay = item.CreationDate });
                mapper.Certificates = listCertificate;
            }

            listExam = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                 .Where(x => x.ChildrenProfileId == childrenId)
                 .GroupBy(x => x.ExamId)
                 .Select(group => new ExamViewModelInChildren
                 {
                     ExamId = group.FirstOrDefault().ExamId,
                     ExamName = group.FirstOrDefault()?.Exam.TestName,
                     /*                     ExamDate = group.FirstOrDefault()?.Exam.CreationDate,
                                          Score = group.Sum(x => x.ScorePerQuestion)*/
                 })
                 .ToList();
            mapper.Exams = listExam;
            
            return mapper;

        }

        public async Task<List<ChildrenViewModel>> GetChildrenByParentId(Guid Id)
        {
            var getChildrens = _unitOfWork.ChildrenRepository.GetAllAsync().Result.Where(x => x.UserId == Id).ToList();

            List <ChildrenViewModel> childrenViewModels = new List<ChildrenViewModel> ();

            foreach (var item in getChildrens)
            {
                childrenViewModels.Add(await GetChildrenById(item.Id));
            }
             return childrenViewModels;
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

            if (children == null)
            {
                throw new Exception("Không tìm thấy học sinh này");
            }

            if (children.Status == StatusChildrenProfile.Reserve)
            {
                throw new Exception("Học sinh này đã bảo lưu rồi");
            }


            //delete attendance
            var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByChilIdAndStatusFuture(childrenReserveViewModel.ChildrenProfileId);
            _unitOfWork.AttendanceRepository.RemoveRange(attendances);
            //await _unitOfWork.SaveChangeAsync();

            //delete enrolled
            var enrolled = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByChildId(childrenReserveViewModel.ChildrenProfileId);
            foreach (var enrollment in enrolled)
            {
                //update actualnumber
                var classed = await _unitOfWork.ClassRepository.GetByIdAsync(enrollment.ClassId);
                classed.ActualNumber = classed.ActualNumber - 1;
                _unitOfWork.ClassRepository.Update(classed);
                //await _unitOfWork.SaveChangeAsync();
            }
           //_unitOfWork.EnrollmentRepository.SoftRemoveRange(enrolled);
           //await _unitOfWork.SaveChangeAsync();

            children.Status = StatusChildrenProfile.Reserve;
            _unitOfWork.ChildrenRepository.Update(children);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bảo lưu thất bại");

        }

        public async Task<ChildrenSummariseViewModel> GetChildrenSummariseViewModel(DateTime MonthAndYear)
        {
            try
            {
                var childrenSummarise = new ChildrenSummariseViewModel();
                var childs = await _unitOfWork.ChildrenRepository.GetAllAsync();
                childrenSummarise.TotalChildren = childs.Count;

                for (int month = 1; month <= MonthAndYear.Month; month++)
                {
                    var monthStart = new DateTime(MonthAndYear.Year, month, 1);

                    var childInMonth = await _unitOfWork.ChildrenRepository.GetChildrenProfiles(monthStart);

       
                    var childsByMonth = new ChildrenSummariseByMonthViewModel
                    {
                        totalByMonth = childInMonth.Count,
                        childrens = _mapper.Map<List<ChildrenProfileViewModel>>(childInMonth),
                    };

                    
                }

                return childrenSummarise;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving children summary information: " + ex.Message);
            }
        }

        //gợi ý khoá học
        public async Task<List<CourseViewModel>> CourseSuggestions(Guid childrenId)
        {
            var getAge = await _unitOfWork.ChildrenRepository.GetByIdAsync(childrenId);

            var getCourse = await _unitOfWork.CourseRepository.GetAllAsync();

            var scoreEntrance = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                 .Where(x => x.ChildrenProfileId == childrenId && x.Exam.TestType == TestType.Entrance).Sum(x => x.ScorePerQuestion);

            List<CourseViewModel> listCourseViewModel = new List<CourseViewModel>();

            foreach (var item in getCourse)
            {
                MatchCollection matches = Regex.Matches(item.Description, @"\d+");

                int startAge, endAge;

                if (matches.Count >= 2)
                {
                    if (int.TryParse(matches[0].Value, out startAge) && int.TryParse(matches[1].Value, out endAge))
                    {
                        if ((_currentTime.GetCurrentTime().Year - getAge.BirthDay.Year) >= startAge 
                            && (_currentTime.GetCurrentTime().Year - getAge.BirthDay.Year) <= endAge 
                            && item.EntryPoint  <= scoreEntrance)
                        {
                            listCourseViewModel.Add(_mapper.Map<CourseViewModel>(item));
                        }
                    }                   
                }
            }

            if (listCourseViewModel.Count <= 0)
            {
                return _mapper.Map<List<CourseViewModel>>(getCourse);
            }

            return listCourseViewModel;
        }

        public async Task<List<ChildrenViewModel>> GetListChildrenByOutClassId(Guid classId)
        {
            var allChilds = await _unitOfWork.ChildrenRepository.GetAllAsync();
            var childs = await _unitOfWork.ChildrenRepository.GetListChildrenProfileByClassId(classId);
            var notEnrolledChildren = allChilds.Where(x => !childs.Select(e => e.Id).Contains(x.Id)).ToList();
            return _mapper.Map<List<ChildrenViewModel>>(notEnrolledChildren); ;
        }
    }
}
