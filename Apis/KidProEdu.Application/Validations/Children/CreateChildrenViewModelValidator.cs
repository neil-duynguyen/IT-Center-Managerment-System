using FluentValidation;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Children
{
    public class CreateChildrenViewModelValidator : AbstractValidator<CreateChildrenViewModel>
    {
        public CreateChildrenViewModelValidator() { 
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Họ và tên không được để trống.").
                MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.BirthDay).NotEmpty().WithMessage("Ngày sinh không được để trống.");
        }
    }
}
