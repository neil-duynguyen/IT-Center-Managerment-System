using FluentValidation;
using KidProEdu.Application.ViewModels.LocationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Locations
{
    public class CreateLocationViewModelValidator : AbstractValidator<CreateLocationViewModel>
    {
        public CreateLocationViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được để trống.")
                .MaximumLength(100).WithMessage("Địa chỉ không quá 100 ký tự.");
        }


    }
}
