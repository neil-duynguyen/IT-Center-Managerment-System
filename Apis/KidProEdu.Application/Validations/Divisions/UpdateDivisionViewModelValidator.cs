using FluentValidation;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Divisions
{

    public class UpdateDivisionViewModelValidator : AbstractValidator<UpdateDivisionViewModel>
    {
        public UpdateDivisionViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không quá 1000 ký tự.");
        }


    }
}
