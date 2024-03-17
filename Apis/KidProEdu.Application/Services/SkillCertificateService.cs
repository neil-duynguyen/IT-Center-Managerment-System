using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.SkillCertificates;
using KidProEdu.Application.Validations.Skills;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class SkillCertificateService : ISkillCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public SkillCertificateService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateSkillCertificate(CreateSkillCertificateViewModel createSkillCertificateViewModel)
        {
            var validator = new CreateSkillCertificateViewModelValidator();
            var validationResult = validator.Validate(createSkillCertificateViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var skill = await _unitOfWork.SkillCertificateRepository.GetSkillCertificateByUserAccountIdAndSkillId(createSkillCertificateViewModel.UserAccountId, createSkillCertificateViewModel.SkillId);
            if (skill != null)
            {
                throw new Exception("Liên kết này đã tồn tại");
            }

            var mapper = _mapper.Map<SkillCertificate>(createSkillCertificateViewModel);
            await _unitOfWork.SkillCertificateRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo liên kết thất bại");
        }

        public async Task<bool> DeleteSkillCertificate(Guid id)
        {
            var result = await _unitOfWork.SkillCertificateRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy liên kết này");
            else
            {
                _unitOfWork.SkillCertificateRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa liên kết thất bại");
            }
        }

        public async Task<List<SkillCertificateViewModel>> GetListSkillCertificatesByUserAccountId(Guid id)
        {
            var result = await _unitOfWork.SkillCertificateRepository.GetListSkillCertificatesByUserAccountId(id);
            var mapper = _mapper.Map<List<SkillCertificateViewModel>>(result);
            return mapper;
        }

        public async Task<SkillCertificateViewModel> GetSkillCertificateById(Guid id)
        {
            var result = await _unitOfWork.SkillCertificateRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<SkillCertificateViewModel>(result);
            return mapper;
        }

        public async Task<List<SkillCertificateViewModel>> GetSkillCertificates()
        {
            var result = _unitOfWork.SkillCertificateRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<SkillCertificateViewModel>>(result);
            return mapper;
        }

        public async Task<bool> UpdateSkillCertificate(UpdateSkillCertificateViewModel updateSkillCertificateViewModel)
        {
            var validator = new UpdateSkillCertificateViewModelValidator();
            var validationResult = validator.Validate(updateSkillCertificateViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var skillCertificate = await _unitOfWork.SkillCertificateRepository.GetByIdAsync(updateSkillCertificateViewModel.Id);
            if (skillCertificate == null)
            {
                throw new Exception("Không tìm thấy liên kết");
            }


            var existingSkillCertificate = await _unitOfWork.SkillCertificateRepository.GetSkillCertificateByUserAccountIdAndSkillId(updateSkillCertificateViewModel.UserAccountId, updateSkillCertificateViewModel.SkillId);
            if (existingSkillCertificate != null)
            {
                if (existingSkillCertificate.Id != updateSkillCertificateViewModel.Id)
                {
                    throw new Exception("Liên kết này đã tồn tại");
                }
            }

            var mapper = _mapper.Map<SkillCertificate>(skillCertificate);
            mapper.UserAccountId = updateSkillCertificateViewModel.UserAccountId;
            mapper.SkillId = updateSkillCertificateViewModel.SkillId;
            mapper.Url = updateSkillCertificateViewModel.Url;
            _unitOfWork.SkillCertificateRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật liên kết thất bại");
        }
    }
}
