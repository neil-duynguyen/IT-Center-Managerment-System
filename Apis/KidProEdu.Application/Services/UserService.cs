using AutoMapper;
using AutoMapper.Execution;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Users;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Linq.Expressions;

namespace KidProEdu.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IClaimsService claimsService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _claimsService = claimsService;
        }

        public async Task<LoginViewModel> LoginAsync(UserLoginViewModel userObject)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAndPasswordHash(userObject.UserName, userObject.Password.Hash());
            if (user == null)
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            var token = user.GenerateJsonWebToken(_configuration["AppSettings:SecretKey"]);

            var userViewModel = _mapper.Map<LoginViewModel>(user);
            userViewModel.Token = token;

            return userViewModel;
        }

        public async Task<bool> CreateAccountAsync(CreateUserViewModel userObject)
        {
            // check username exited
            var isExited = await _unitOfWork.UserRepository.CheckUserNameExited(userObject);

            /* var newUser = new User
             {
                 RoleId = userObject.RoleId,
                 UserName = userObject.UserName,
                 PasswordHash = new String("123").Hash(),
                 FullName = userObject.FullName,
                 GenderType = userObject.GenderType,
                 Email = userObject.Email,
                 Phone = userObject.Phone,
                 Address = userObject.Address,
                 CreationDate = _currentTime.GetCurrentTime(),
             };*/

            var newUser = _mapper.Map<UserAccount>(userObject);
            newUser.PasswordHash = newUser.PasswordHash.Hash();
            newUser.Status = Domain.Enums.StatusUser.Enable;

            await _unitOfWork.UserRepository.AddAsync(newUser);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var findUser = await _unitOfWork.UserRepository.GetByIdAsync(id);

            return findUser != null ? _mapper.Map<UserViewModel>(findUser) : throw new Exception();
        }

        public async Task<List<UserViewModel>> GetAllUser()
        {
            var user = _unitOfWork.UserRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate);

            return _mapper.Map<List<UserViewModel>>(user);
        }

        public async Task<List<UserViewModel>> GetUserByRoleId(Guid Id)
        {
            var user = _unitOfWork.UserRepository.GetAllAsync().Result.Where(x => x.RoleId == Id && x.Status.ToString().Equals("Enable"));

            return _mapper.Map<List<UserViewModel>>(user);
        }

        public async Task<UserViewModel> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(changePasswordViewModel.id) ?? throw new Exception("Không tìm thấy người dùng");

            if (changePasswordViewModel.currentPassword != null)
            {
                if (!user.PasswordHash.Equals(changePasswordViewModel.currentPassword.Hash()))
                    throw new Exception("Mật khẩu hiện tại không đúng");
            }

            user.PasswordHash = (changePasswordViewModel.newPasswordHash).Hash();

            _unitOfWork.UserRepository.Update(user);

            return await _unitOfWork.SaveChangeAsync() > 0 ? _mapper.Map<UserViewModel>(user) : throw new Exception("Thay đổi mật khẩu không thành công");
        }

        public async Task<bool> UpdateUser(UpdateUserViewModel updateUserViewModel, params Expression<Func<UserAccount, object>>[] uniqueProperties)
        {
            var validator = new UpdateUserViewModelValidator();
            var validationResult = validator.Validate(updateUserViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            foreach (var property in uniqueProperties)
            {
                var userAccount = await _unitOfWork.UserRepository.GetUserAccountByProperty(updateUserViewModel, property);
                if (userAccount != null && userAccount.Id != updateUserViewModel.Id)
                {
                    throw new Exception($"{property.GetMember().Name} đã tồn tại");
                }
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(updateUserViewModel.Id) ?? throw new Exception("Không tìm thấy người dùng này");

            _unitOfWork.UserRepository.Update(_mapper.Map(updateUserViewModel, user));

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thông tin không thành công");
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var result = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (id == _claimsService.GetCurrentUserId)
            {
                throw new Exception("Không thể tự xóa bản thân");
            }
            else if (result == null)
            {
                throw new Exception("Không tìm thấy người dùng này");
            }
            else
            {
                _unitOfWork.UserRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa người dùng thất bại");
            }
        }

        public async Task<bool> ChangeStatusUser(Guid[] listId)
        {
            foreach (var item in listId)
            {
                if (item == _claimsService.GetCurrentUserId)
                {
                    continue;
                }
                else
                {
                    var user = await _unitOfWork.UserRepository.GetByIdAsync(item);

                    if (user.Status.ToString().Equals("Enable"))
                        user.Status = Domain.Enums.StatusUser.Disable;
                    else user.Status = Domain.Enums.StatusUser.Enable;

                    _unitOfWork.UserRepository.Update(user);
                }
            }
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trạng thái người dùng thất bại");
        }

    }
}
