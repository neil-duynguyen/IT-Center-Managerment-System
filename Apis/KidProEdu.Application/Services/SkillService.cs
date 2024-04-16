using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.Validations.Schedules;
using KidProEdu.Application.Validations.Skills;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public SkillService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateSkill(CreateSkillViewModel createSkillViewModel)
        {
            var validator = new CreateSkillViewModelValidator();
            var validationResult = validator.Validate(createSkillViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var skill = await _unitOfWork.SkillRepository.GetSkillByName(createSkillViewModel.Name);
            if (skill != null)
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<Skill>(createSkillViewModel);
            await _unitOfWork.SkillRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo kĩ năng thất bại");
        }

        public async Task<bool> DeleteSkill(Guid id)
        {
            var result = await _unitOfWork.SkillRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy kĩ năng này");
            else
            {
                _unitOfWork.SkillRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa kĩ năng thất bại");
            }
        }

        public async Task<SkillViewModel> GetSkillById(Guid id)
        {
            var result = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<SkillViewModel>(result);
            return mapper;
        }

        public async Task<SkillViewModel> GetSkillByName(string name)
        {
            var result = await _unitOfWork.SkillRepository.GetSkillByName(name);
            var mapper = _mapper.Map<SkillViewModel>(result);
            return mapper;
        }

        public async Task<List<SkillViewModel>> GetSkills()
        {
            var result = _unitOfWork.SkillRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<SkillViewModel>>(result);
            return mapper;
        }

        public async Task<bool> UpdateSkill(UpdateSkillViewModel updateSkillViewModel)
        {
            var validator = new UpdateSkillViewModelValidator();
            var validationResult = validator.Validate(updateSkillViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(updateSkillViewModel.Id);
            if (skill == null)
            {
                throw new Exception("Không tìm thấy kĩ năng");
            }


            var existingSkill = await _unitOfWork.SkillRepository.GetSkillByName(updateSkillViewModel.Name);
            if (existingSkill != null)
            {
                if (existingSkill.Id != updateSkillViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }


            skill.Name = updateSkillViewModel.Name;
            skill.Description = updateSkillViewModel.Description;
            _unitOfWork.SkillRepository.Update(skill);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật kĩ năng thất bại");
        }
    }
}
