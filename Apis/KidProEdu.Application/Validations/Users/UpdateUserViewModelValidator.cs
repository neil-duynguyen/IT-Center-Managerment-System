using FluentValidation;
using KidProEdu.Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Users
{
    public class UpdateUserViewModelValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateUserViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không thể bỏ trống");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username không thể bỏ trống");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Fullname không thể bỏ trống");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không thể bỏ trống");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone không thể bỏ trống");
            //RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("DateOfBirth không thể bỏ trống");
            RuleFor(x => x.GenderType).NotEmpty().WithMessage("GenderType không thể bỏ trống");
            //RuleFor(x => x.Avatar).NotEmpty().WithMessage("Avatar không thể bỏ trống");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address không thể bỏ trống");
            
            
        }
    }
}
