using AutoMapper;
using FluentValidation;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Blogs;
using KidProEdu.Application.Validations.Course;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;

        public CourseService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IClaimsService claimsService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _claimsService = claimsService;
        }

        //Tạo con
        public async Task<bool> CreateCourseAsync(CreateCourseViewModel createCourseViewModel)
        {
            var validator = new CreateCourseViewModelValidator();
            var validationResult = validator.Validate(createCourseViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var checkCourseType = _unitOfWork.CourseRepository.GetByIdAsync(createCourseViewModel.ParentCourse).Result.CourseType.Equals(CourseType.Single) ? throw new Exception("Khoá học đơn không thể tạo khoá học con.") : true;

            // check name exited
            var isExitedName = await _unitOfWork.CourseRepository.CheckNameExited(createCourseViewModel.Name) ? throw new Exception("Tên khoá học đã tồn tại.") : true;
            var isExitedCode = _unitOfWork.CourseRepository.GetAllAsync().Result.Any(x => x.CourseCode == createCourseViewModel.CourseCode) ? throw new Exception("Mã khoá học đã tồn tại.") : true;     

            var getCourse = await _unitOfWork.CourseRepository.GetAllAsync();
            var mapper = _mapper.Map<Course>(createCourseViewModel);
            mapper.Level = getCourse.Where(x => x.ParentCourse == createCourseViewModel.ParentCourse).Count() + 1;
            mapper.CourseType = Domain.Enums.CourseType.Single;

            await _unitOfWork.CourseRepository.AddAsync(mapper);

            var getCourseByParentCourse = getCourse.FirstOrDefault(x => x.Id == createCourseViewModel.ParentCourse && x.ParentCourse is null);
            getCourseByParentCourse.DurationTotal = (getCourse.Where(x => x.ParentCourse == createCourseViewModel.ParentCourse).Sum(x => x.DurationTotal)) + createCourseViewModel.DurationTotal;
            getCourseByParentCourse.Price = (getCourse.Where(x => x.ParentCourse == createCourseViewModel.ParentCourse).Sum(x => x.Price)) + createCourseViewModel.Price;
            _unitOfWork.CourseRepository.Update(getCourseByParentCourse);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        //Tạo cha
        public async Task<bool> CreateCourseParentAsync(CreateCourseParentViewModel createCourseParentViewModel)
        {
            var validator = new CreateCourseParentViewModelValidator();
            var validationResult = validator.Validate(createCourseParentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            // check name exited
            var isExitedName = await _unitOfWork.CourseRepository.CheckNameExited(createCourseParentViewModel.Name) ? throw new Exception("Tên khoá học đã tồn tại.") : true;
            var isExitedCode = _unitOfWork.CourseRepository.GetAllAsync().Result.Any(x => x.CourseCode == createCourseParentViewModel.CourseCode) ? throw new Exception("Mã khoá học đã tồn tại."): true;

            var mapper = _mapper.Map<Course>(createCourseParentViewModel);

            await _unitOfWork.CourseRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        public async Task<CourseViewModel> GetCourseById(Guid Id)
        {
            var courseParent = await _unitOfWork.CourseRepository.GetByIdAsync(Id);
            var mapper = _mapper.Map<CourseViewModel>(courseParent);

            var getList = _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.ParentCourse == courseParent.Id && x.IsDeleted == false).ToList();
            var classes = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.CourseId == Id && x.IsDeleted == false).ToList();
            var lessons = _unitOfWork.LessonRepository.GetAllAsync().Result.Where(x => x.CourseId == Id && x.IsDeleted == false).ToList();

            mapper.Courses = _mapper.Map<List<CourseViewModel>>(getList);
            mapper.Classes = _mapper.Map<List<ClassViewModel>>(classes);
            mapper.Lessons = _mapper.Map<List<LessonViewModel>>(lessons);

            return mapper;
        }

        public async Task<List<CourseViewModel>> GetAllCourse()
        {
            var resultt = _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var listCourseViewModel = new List<CourseViewModel>();

            foreach (var item in resultt)
            {
                var listCourse = new List<CourseViewModel>();

                if (item.CourseType == Domain.Enums.CourseType.Spect)
                {
                    var mapperParentCourse = _mapper.Map<CourseViewModel>(item);

                    var result = _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.ParentCourse == item.Id && x.IsDeleted == false).ToList();
                    mapperParentCourse.Courses = _mapper.Map<List<CourseViewModel>>(result);
                    listCourseViewModel.Add(mapperParentCourse);
                }
                if (item.ParentCourse is null && item.CourseType == Domain.Enums.CourseType.Single && item.IsDeleted == false)
                {
                    var course = _mapper.Map<CourseViewModel>(item);
                    course.Courses = listCourse.Count != 0 ? listCourse : null;
                    listCourseViewModel.Add(course);
                }
            }

            return listCourseViewModel;
        }

        public async Task<bool> DeleteCourseAsync(Guid courseId)
        {
            var getCourse = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);

            if (getCourse == null) throw new Exception("Không tìm thấy Course");

            _unitOfWork.CourseRepository.SoftRemove(getCourse);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByChildId(Guid childId)
        {
            var courses = await _unitOfWork.CourseRepository.GetListCourseByChildrenId(childId);
            var mapper = _mapper.Map<List<CourseViewModel>>(courses);
            return mapper;
        }

        public async Task<bool> UpdateCourseAsync(UpdateCourseViewModel updateCourseViewModel)
        {
            var validator = new UpdateCourseViewModelValidator();
            var validationResult = validator.Validate(updateCourseViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            //check duplicate course name
            var checkName = _unitOfWork.CourseRepository.GetAllAsync().Result.Any(x => x.Id != updateCourseViewModel.Id && x.Name == updateCourseViewModel.Name) ? throw new Exception("Tên khoá học đã tồn tại.") : true;
            var checkCode = _unitOfWork.CourseRepository.GetAllAsync().Result.Any(x => x.Id != updateCourseViewModel.Id && x.CourseCode == updateCourseViewModel.CourseCode) ? throw new Exception("Mã khoá học đã tồn tại.") : true;

            var getCourse = await _unitOfWork.CourseRepository.GetByIdAsync(updateCourseViewModel.Id);
            var mapper = _mapper.Map(updateCourseViewModel, getCourse);
            _unitOfWork.CourseRepository.Update(mapper);
            await _unitOfWork.SaveChangeAsync();

            var getCourseParent = await _unitOfWork.CourseRepository.GetByIdAsync((Guid)getCourse.ParentCourse);
            var getListCourseByParentId = await _unitOfWork.CourseRepository.GetAllAsync();
            getCourseParent.DurationTotal = (getListCourseByParentId.Where(x => x.ParentCourse == getCourseParent.Id).Sum(x => x.DurationTotal));
            getCourseParent.Price = (getListCourseByParentId.Where(x => x.ParentCourse == getCourseParent.Id).Sum(x => x.Price));
            _unitOfWork.CourseRepository.Update(getCourseParent);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật khoá học thất bại");
        }


        public async Task<bool> UpdateCourseParentAsync(UpdateCourseParentViewModel updateCourseParentViewModel)
        {
            var validator = new UpdateCourseParentViewModelValidator();
            var validationResult = validator.Validate(updateCourseParentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            //check duplicate course name
            var checkName = _unitOfWork.CourseRepository.GetAllAsync().Result.Any(x => x.Id != updateCourseParentViewModel.Id && x.Name == updateCourseParentViewModel.Name) ? throw new Exception("Tên khoá học đã tồn tại.") : true;
            var checkCode = _unitOfWork.CourseRepository.GetAllAsync().Result.Any(x => x.Id != updateCourseParentViewModel.Id && x.CourseCode == updateCourseParentViewModel.CourseCode) ? throw new Exception("Mã khoá học đã tồn tại.") : true;
            
            var getCourse = await _unitOfWork.CourseRepository.GetByIdAsync(updateCourseParentViewModel.Id);
            var mapper = _mapper.Map(updateCourseParentViewModel, getCourse);
            _unitOfWork.CourseRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật khoá học thất bại");
        }
    }
}
