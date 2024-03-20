using FluentValidation;
using KidProEdu.Application.ViewModels.SkillViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Skills
{

    public class UpdateSkillViewModelValidator : AbstractValidator<UpdateSkillViewModel>
    {
        public UpdateSkillViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(200).WithMessage("Tên không quá 200 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Tên không quá 1000 ký tự.");
        }


    }
}
