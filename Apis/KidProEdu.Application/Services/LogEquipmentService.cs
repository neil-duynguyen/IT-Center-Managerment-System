using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Lessons;
using KidProEdu.Application.Validations.LogEquipments;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZXing.QrCode.Internal;
using static ZXing.QrCode.Internal.Mode;

namespace KidProEdu.Application.Services
{
    public class LogEquipmentService : ILogEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public LogEquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> DeleteLogEquipment(Guid id)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy log thiết bị");
            else
            {
                _unitOfWork.LogEquipmentRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa log thiết bị thất bại");
            }
        }

        public async Task<LogEquipmentViewModel> GetLogEquipmentById(Guid id)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<LogEquipmentViewModel>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentByStatus(StatusOfEquipment statusOfEquipment)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByStatus(statusOfEquipment);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipments()
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetAllAsync();
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByCateEquipmentId(Guid cateId)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentsByCateEquipmentId(cateId);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByCode(string code)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByCode(code);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByEquipmentId(Guid equipmentId)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByEquipmentId(equipmentId);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByName(string name)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByName(name);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByRoomId(Guid roomId)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByRoomId(roomId);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByUserId(Guid userId)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByUserId(userId);
            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(result);
            return mapper;
        }

    }
}
