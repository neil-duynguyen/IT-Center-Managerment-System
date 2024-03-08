using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Domain.Entities;
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

        public async Task<bool> CreateCourseAsync(CreateCourseViewModel createCourseViewModel)
        {
            // check name exited
            var isExited = await _unitOfWork.CourseRepository.CheckNameExited(createCourseViewModel.Name);

            if (isExited)
            {
                throw new Exception("Tên Course đã tồn tại.");
            }

            var mapper = _mapper.Map<Course>(createCourseViewModel);

            await _unitOfWork.CourseRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        public async Task<bool> CreateCourseParentAsync(CreateCourseParentViewModel createCourseParentViewModel)
        {
            // check name exited
            var isExited = await _unitOfWork.CourseRepository.CheckNameExited(createCourseParentViewModel.Name);

            if (isExited)
            {
                throw new Exception("Tên Course đã tồn tại.");
            }

            var mapper = _mapper.Map<Course>(createCourseParentViewModel);

            await _unitOfWork.CourseRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        public async Task<CourseViewModel> GetCourseById(Guid Id)
        {
            var courseParent = await _unitOfWork.CourseRepository.GetByIdAsync(Id);
            var mapper = _mapper.Map<CourseViewModel>(courseParent);

            var getList = _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.ParentCourse == courseParent.Id).ToList();

            mapper.Courses = _mapper.Map<List<CourseViewModel>>(getList);

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
                    var result = _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.ParentCourse == item.Id).ToList();
                    listCourse = _mapper.Map<List<CourseViewModel>>(result);
                }
                if (item.ParentCourse.Equals(Guid.Empty) || item.CourseType == Domain.Enums.CourseType.Spect)
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

       /* public async Task<bool> UpdateCourseAsync(CreateCourseViewModel createCourseViewModel)
        {
            //check duplicate course name
            var checkName = await _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.Id != );

            checkName.FirstOrDefault(x => x.Name.Equals(createCourseViewModel.Name, StringComparison.OrdinalIgnoreCase));


            return true;
        }*/
    }
}
