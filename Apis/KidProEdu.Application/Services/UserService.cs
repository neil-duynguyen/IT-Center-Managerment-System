using AutoMapper;
using AutoMapper.Execution;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Users;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace KidProEdu.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }

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
            newUser.PasswordHash = new String("User@123").Hash();
            newUser.Status = Domain.Enums.StatusUser.Enable;
            //newUser.LocationId = _unitOfWork.UserRepository.GetByIdAsync(_claimsService.GetCurrentUserId).Result.LocationId;

            await _unitOfWork.UserRepository.AddAsync(newUser);

            if (userObject.createContractViewModel != null)
            {
                ContractService sv = new ContractService(_unitOfWork, _currentTime, _claimsService, _mapper);
                await sv.CreateContract(userObject.createContractViewModel, newUser.Id);
            }

            SendEmailUtil.SendEmail(newUser.Email, "Thông báo đăng kí tài khoản KidProEdu",
                "Thông tin tài khoản\n " +
                "UserName: " + newUser.UserName +
                "\nPassword: User@123");


            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var findUser = await _unitOfWork.UserRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<UserViewModel>(findUser);

            if (findUser.Role.Name.Equals("Parent"))
            {

                var getChildren = _unitOfWork.ChildrenRepository.GetAllAsync().Result.Where(x => x.UserId == findUser.Id).ToList();

                mapper.ChildrenProfiles = _mapper.Map<List<ChildrenViewModel>>(getChildren);
            }

            return findUser != null ? mapper : throw new Exception();
        }

        public async Task<List<UserViewModel>> GetAllUser()
        {
            var listUser = await _unitOfWork.UserRepository.GetAllAsync();

            var getCurrentUserId = await _unitOfWork.UserRepository.GetByIdAsync(_claimsService.GetCurrentUserId);

            List<UserAccount> users = new List<UserAccount>();

            if (getCurrentUserId.Role.Name.Equals("Admin") && getCurrentUserId.UserName.Equals("Admin"))
            {
                users = listUser.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreationDate).ToList();
            }

            if (getCurrentUserId.Role.Name.Equals("Admin") && !getCurrentUserId.UserName.Equals("Admin"))
            {
                users = listUser.Where(x => x.Role.Name.Equals("Manager") || x.Role.Name.Equals("Teacher") || x.Role.Name.Equals("Staff") || x.Role.Name.Equals("Parent") && !x.IsDeleted).OrderByDescending(x => x.CreationDate).ToList();
            }

            if (getCurrentUserId.Role.Name.Equals("Manager"))
            {
                users = listUser.Where(x => x.Role.Name.Equals("Teacher") || x.Role.Name.Equals("Staff") && !x.IsDeleted).OrderByDescending(x => x.CreationDate).ToList();
            }

            if (getCurrentUserId.Role.Name.Equals("Staff"))
            {
                users = listUser.Where(x => x.Role.Name.Equals("Parent") && !x.IsDeleted && x.CreatedBy == _claimsService.GetCurrentUserId).OrderByDescending(x => x.CreationDate).ToList();
            }

            return _mapper.Map<List<UserViewModel>>(users);
        }

        public async Task<List<UserViewModel>> GetUserByRoleId(Guid Id)
        {
            var user = _unitOfWork.UserRepository.GetAllAsync().Result.Where(x => x.RoleId == Id && x.IsDeleted == false).OrderByDescending(x => x.CreationDate);

            return _mapper.Map<List<UserViewModel>>(user);
        }

        // vừa change pass vừa reset pass
        public async Task<UserViewModel> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(changePasswordViewModel.id) ?? throw new Exception("Không tìm thấy người dùng");

            if (changePasswordViewModel.currentPassword != null)
            {
                if (!user.PasswordHash.Equals(changePasswordViewModel.currentPassword.Hash()))
                    throw new Exception("Mật khẩu hiện tại không đúng");
                else
                {
                    user.PasswordHash = (changePasswordViewModel.newPasswordHash).Hash();

                    _unitOfWork.UserRepository.Update(user);

                    return await _unitOfWork.SaveChangeAsync() > 0 ? _mapper.Map<UserViewModel>(user) : throw new Exception("Thay đổi mật khẩu không thành công");
                }
            }
            else
            {
                user.PasswordHash = (changePasswordViewModel.newPasswordHash).Hash();

                _unitOfWork.UserRepository.Update(user);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    await SendEmailUtil.SendEmail(user.Email, "Thông báo bảo mật tài khoản",
                        "KidPro thông báo, \n\n" +
                        "Tài khoản của quý khách vừa được đặt lại mật khẩu, mật khẩu hiện tại của bạn là: User@123\n" +
                        "Trân trọng, \n" +
                        "KidPro Education!");
                    return _mapper.Map<UserViewModel>(user);

                }
                else
                {
                    throw new Exception("Đặt lại mật khẩu không thành công");

                }
            }
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

        public async Task<List<UserViewModel>> GetTeacherFree()
        {
            List<UserViewModel> listUser = new();

            var listTeacher = _unitOfWork.UserRepository.GetAllAsync().Result.Where(x => x.Role.Name == "Teacher").ToList();

            foreach (var item in listTeacher)
            {
                var checkFree = await _unitOfWork.TeachingClassHistoryRepository.GetClassByTeacherId(item.Id);
                if (checkFree.IsNullOrEmpty())
                {
                    listUser.Add(_mapper.Map<UserViewModel>(item));
                }
            }

            return listUser;
        }
    }
}
