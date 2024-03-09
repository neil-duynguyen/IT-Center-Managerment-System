using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Classes;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace KidProEdu.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ClassService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateClass(CreateClassViewModel createClassViewModel)
        {
            var validator = new CreateClassViewModelValidator();
            var validationResult = validator.Validate(createClassViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var findClass = await _unitOfWork.ClassRepository.GetClassByClassCode(createClassViewModel.ClassCode);
            if (!findClass.IsNullOrEmpty())
            {
                throw new Exception("Lớp đã tồn tại");
            }

            var mapper = _mapper.Map<Class>(createClassViewModel);

            mapper.StatusOfClass = Domain.Enums.StatusOfClass.Pending;
            mapper.ActualNumber = 0;

            await _unitOfWork.ClassRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo lớp thất bại");

        }

        public async Task<bool> DeleteClass(Guid ClassId)
        {
            var result = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);

            if (result == null)
                throw new Exception("Không tìm thấy lớp này");
            else
            {
                _unitOfWork.ClassRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa lớp thất bại");
            }
        }

        public async Task<ClassViewModel> GetClassById(Guid ClassId)
        {
            var getClass = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            return _mapper.Map<ClassViewModel>(getClass); ;
        }

        public async Task<List<ClassViewModel>> GetClasses()
        {
            var Classs = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<ClassViewModel>>(Classs);
        }

        public async Task<List<ClassViewModel>> GetClassBySemester(Guid id)
        {
            var Classs = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false && x.SemesterId == id).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<ClassViewModel>>(Classs);
        }

        public async Task<bool> UpdateClass(UpdateClassViewModel updateClassViewModel)
        {
            var validator = new UpdateClassViewModelValidator();
            var validationResult = validator.Validate(updateClassViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(updateClassViewModel.Id)
                ?? throw new Exception("Không tìm thấy lớp");

            if (updateClassViewModel.MaxNumber < findClass.ActualNumber)
            {
                throw new Exception("Số lượng học sinh tối đa không thể nhỏ hơn số lượng học sinh thực tế đang có trong lớp");
            }
            /*else if (updateClassViewModel.MaxNumber < findClass.ExpectedNumber)
            {
                throw new Exception("Số lượng học sinh dự kiến không thể lớn hơn số lượng học tối đa của lớp");
            }*/

            var existingClass = await _unitOfWork.ClassRepository.GetClassByClassCode(updateClassViewModel.ClassCode);
            if (!existingClass.IsNullOrEmpty())
            {
                foreach (var item in existingClass)
                {
                    if (item.Id != updateClassViewModel.Id)
                    {
                        throw new Exception("Lớp đã tồn tại");
                    }
                }
            }

            if (findClass.StatusOfClass != Domain.Enums.StatusOfClass.Pending)
                throw new Exception("Cập nhật lớp thất bại, lớp này hiện không còn trong trạng thái chờ");

            /*Class.ClassName = updateClassViewModel.ClassName;
            Class.Description = updateClassViewModel.Description;*/

            var mapper = _mapper.Map(updateClassViewModel, findClass);

            _unitOfWork.ClassRepository.Update(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật lớp thất bại");
        }

        public async Task<bool> ChangeStatusClass(ChangeStatusClassViewModel changeStatusClassViewModel)
        {
            var status = Domain.Enums.StatusOfClass.Pending;

            status = changeStatusClassViewModel.status switch
            {
                "Started" => (Domain.Enums.StatusOfClass)Domain.Enums.StatusOfClass.Started,
                //"Pending" => Domain.Enums.StatusOfClass.Pending,
                "Cancel" => Domain.Enums.StatusOfClass.Cancel,
                "Expired" => Domain.Enums.StatusOfClass.Expired,
                _ => throw new Exception("Trạng thái không được hỗ trợ"),
            };

            foreach (var item in changeStatusClassViewModel.ids)
            {
                var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(item);

                if (findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Cancel)
                    || findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Expired))
                    continue;

                findClass.StatusOfClass = status;

                _unitOfWork.ClassRepository.Update(findClass);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trạng thái lớp thất bại");
        }

        //api này dùng để get childen trong classid
        public async Task<List<ClassChildrenViewModel>> GetChildrenByClassId(Guid classId)
        {
            var result = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).ToList();
            var mapper = _mapper.Map<List<ClassChildrenViewModel>>(result);
            return mapper;
        }
    }
}
