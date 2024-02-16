using AutoMapper;
using AutoMapper.Execution;
using FluentValidation.Internal;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.TrainingPrograms;
using KidProEdu.Application.ViewModels.TrainingProgramViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public TrainingProgramService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateTrainingProgram(CreateTrainingProgramViewModel createTrainingProgramViewModel)
        {
            var validator = new CreateTrainingProgramViewModelValidator();
            var validationResult = validator.Validate(createTrainingProgramViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByTrainingProgramName(createTrainingProgramViewModel.TrainingProgramName);
            if (trainingProgram != null)
            {
                throw new Exception("Tên đã tồn tại");
            }

            trainingProgram = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByTrainingProgramCode(createTrainingProgramViewModel.TrainingProgramCode);
            if (trainingProgram != null)
            {
                throw new Exception("Mã đã tồn tại");
            }

            var mapper = _mapper.Map<TrainingProgram>(createTrainingProgramViewModel);
            await _unitOfWork.TrainingProgramRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo TrainingProgram thất bại");

        }

        public async Task<bool> DeleteTrainingProgram(Guid trainingProgramId)
        {
            var result = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(trainingProgramId);

            if (result == null)
                throw new Exception("Không tìm thấy TrainingProgram này");
            else
            {
                _unitOfWork.TrainingProgramRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa TrainingProgram thất bại");
            }
        }

        public async Task<TrainingProgram> GetTrainingProgramById(Guid trainingProgramId)
        {
            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(trainingProgramId);
            return trainingProgram;
        }

        public async Task<List<TrainingProgram>> GetTrainingPrograms()
        {
            var TrainingPrograms = _unitOfWork.TrainingProgramRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).ToList();
            return TrainingPrograms;
        }

        public async Task<bool> UpdateTrainingProgram(UpdateTrainingProgramViewModel updateTrainingProgramViewModel, params Expression<Func<TrainingProgram, object>>[] uniqueProperties)
        {
            var validator = new UpdateTrainingProgramViewModelValidator();
            var validationResult = validator.Validate(updateTrainingProgramViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            foreach (var property in uniqueProperties)
            {
                var TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByProperty(updateTrainingProgramViewModel, property);
                if (TrainingProgram != null && TrainingProgram.Id != updateTrainingProgramViewModel.Id)
                {
                    throw new Exception($"{property.GetMember().Name} đã tồn tại");
                }
            }

            /*var TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByTrainingProgramName(updateTrainingProgramViewModel.TrainingProgramName);
            if (TrainingProgram != null && TrainingProgram.Id != updateTrainingProgramViewModel.Id)
            {
                throw new Exception("Tên đã tồn tại");
            }

            TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByStartDate(updateTrainingProgramViewModel.StartDate);
            if (TrainingProgram != null && TrainingProgram.Id != updateTrainingProgramViewModel.Id)
            {
                throw new Exception("Ngày bắt đầu đã tồn tại");
            }*/

            var existingTrainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(updateTrainingProgramViewModel.Id) ?? throw new Exception("Không tìm thấy học kỳ");
            //var mapper = _mapper.Map<TrainingProgram>(updateTrainingProgramViewModel);
            _unitOfWork.TrainingProgramRepository.Update(_mapper.Map(updateTrainingProgramViewModel, existingTrainingProgram));
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật TrainingProgram thất bại");
        }
    }
}
