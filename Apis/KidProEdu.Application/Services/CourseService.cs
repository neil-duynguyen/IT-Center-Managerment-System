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

        /*public async Task<List<CourseViewModel>> GetAllCourse()
        {
            var resultt = _unitOfWork.CourseRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var listCourseViewModel = new List<CourseViewModel>();

            foreach (var item in resultt)
            {
                var listCourse = new List<CourseViewModel>();

                if (item.ParentCourse != null)
                {
                    
                    foreach (var courseid in item.ParentCourse)
                    {
                        var result = await _unitOfWork.CourseRepository.GetByIdAsync(courseid);
                        listCourse.Add(_mapper.Map<CourseViewModel>(result));
                    }
                }

                var course = _mapper.Map<CourseViewModel>(item);
                course.Courses = listCourse.Count != 0 ? listCourse : null;
                listCourseViewModel.Add(course);
            }

            return listCourseViewModel;
        }*/

        public async Task<bool> DeleteCourseAsync(Guid courseId)
        {
            var getCourse = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);

            if (getCourse == null) throw new Exception("Không tìm thấy Course");

            _unitOfWork.CourseRepository.SoftRemove(getCourse);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }
        
        public async Task<bool> UpdateCourseAsync()
        {
            //check duplicate course name
            /*var checkName = await _unitOfWork.CourseRepository.GetAllAsync();
            if (checkName.FirstOrDefault(x => x.CourseType) == null)
            { 
                
            }*/
            return true;
        }
    }
}
