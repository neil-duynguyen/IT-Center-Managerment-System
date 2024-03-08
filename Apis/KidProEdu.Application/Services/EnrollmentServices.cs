using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Children;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public EnrollmentServices(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel)
        {
            //check number children in class
            var getNumbderChildren = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);

            if (getNumbderChildren.ActualNumber == getNumbderChildren.MaxNumber) throw new Exception($"Lớp học {getNumbderChildren.ClassCode} đã đủ số lượng trẻ.");

            var getPriceClass = _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId).Result.Course.Price;

            //update ActualNumber in class
            var updateActualNumber = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);
            updateActualNumber.ActualNumber = getNumbderChildren.Enrollments.Count + 1;

            var mapper = _mapper.Map<Enrollment>(createEnrollmentViewModel);
            mapper.RegisterDate = _currentTime.GetCurrentTime();
            mapper.Commission = getPriceClass * 0.1;
            mapper.UserId = _claimsService.GetCurrentUserId;

            await _unitOfWork.EnrollmentRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Đăng kí thất bại.");
        }

        public async Task<bool> DeleteEnrollment(Guid idChildren)
        { 
            var getEnrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(idChildren);

            if (getEnrollment is null) throw new Exception("Không tìm thấy Enrollment");

            _unitOfWork.EnrollmentRepository.SoftRemove(getEnrollment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        //Lấy những children mà staff đã đăng kí dành cho staff
        public async Task<List<EnrollmentViewModel>> GetEnrollment()
        {
            var getEnrollment = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.UserId == _claimsService.GetCurrentUserId).ToList();

            var mapper = _mapper.Map<List<EnrollmentViewModel>>(getEnrollment);
            return mapper;
        }

        //dành cho manager
        public async Task<List<EnrollmentViewModel>> GetEnrollmentById(Guid Id)
        {
            var getEnrollment = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.UserId == Id).ToList();

            var mapper = _mapper.Map<List<EnrollmentViewModel>>(getEnrollment);
            return mapper;
        }
    }
}
