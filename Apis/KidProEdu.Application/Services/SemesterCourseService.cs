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
            var getCourseId = _unitOfWork.SemesterCourseRepository.GetAllAsync().Result.Where(x => x.SemesterId == createSemesterCourseView.SemesterId && !x.IsDeleted).Select(x => x.CourseId).ToList();

            /*foreach (var item in createSemesterCourseView.CourseId)
            {
                bool isFound = false;
                foreach (var course in getSemestersCourse)
                {
                    if (item == course)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    await _unitOfWork.SemesterCourseRepository.AddAsync(new SemesterCourse() { SemesterId = createSemesterCourseView.SemesterId, CourseId = item });
                }
            }*/

            //except lấy ra phần tử riêng của 2 mảng
            var result = createSemesterCourseView.CourseId.Except(getCourseId).ToList();

            List<SemesterCourse> listSemesterCourse = new List<SemesterCourse>();

            foreach (var courseId in result)
            {
                listSemesterCourse.Add(new SemesterCourse() { SemesterId = createSemesterCourseView.SemesterId, CourseId = courseId });
            }

            await _unitOfWork.SemesterCourseRepository.AddRangeAsync(listSemesterCourse);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Gán Course thất bại.");
        }

        public async Task<List<CourseViewModel>> GetSemesterCourseById(Guid Id)
        { 
            var listCourseId = _unitOfWork.SemesterCourseRepository.GetAllAsync().Result.Where(x => x.SemesterId == Id && !x.IsDeleted).Select(x => x.CourseId).ToList();

            List<CourseViewModel> listCourse = new List<CourseViewModel>();

            foreach (var courseId in listCourseId)
            {
                listCourse.Add(_mapper.Map<CourseViewModel>(await _unitOfWork.CourseRepository.GetByIdAsync(courseId)));
            }
            return listCourse;
        }

        public async Task<bool> DeleteSemesterCourse(Guid courseId)
        {
            var result = await _unitOfWork.SemesterCourseRepository.GetByIdAsync(courseId);

            if (result == null)
                throw new Exception("Không tìm thấy Course");
            else
            {
                _unitOfWork.SemesterCourseRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xoá Course khỏi kì thất bại.");
            }
        }
    }
}
