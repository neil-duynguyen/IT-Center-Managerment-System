using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.SemesterCourseViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class SemesterCourseService : ISemesterCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public SemesterCourseService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> AddCourse(CreateSemesterCourseViewModel createSemesterCourseView)
        {
            List<SemesterCourse> listSemesterCourse = new List<SemesterCourse>();
            foreach (var course in createSemesterCourseView.CourseId)
            {
               listSemesterCourse.Add(new SemesterCourse() { SemesterId = createSemesterCourseView.SemesterId, CourseId = course});
            }

            await _unitOfWork.SemesterCourseRepository.AddRangeAsync(listSemesterCourse);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Gán Course thất bại.");
        }

        public async Task<List<CourseViewModel>> GetSemesterCourseById(Guid Id)
        { 
            var listCourseId = _unitOfWork.SemesterCourseRepository.GetAllAsync().Result.Where(x => x.SemesterId == Id).Select(x => x.CourseId).ToList();

            List<CourseViewModel> listCourse = new List<CourseViewModel>();

            foreach (var courseId in listCourseId)
            {
                listCourse.Add(_mapper.Map<CourseViewModel>(await _unitOfWork.CourseRepository.GetByIdAsync(courseId)));
            }
            return listCourse;
        }

        public async Task<bool> UpdateCourseInSemester(CreateSemesterCourseViewModel createSemesterCourseView)
        {
            /*var findSemester = _unitOfWork.SemesterCourseRepository.GetAllAsync().Result.Where(x => x.SemesterId == createSemesterCourseView.SemesterId).ToList();*/

            
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thất bại.");
        }
    }
}
