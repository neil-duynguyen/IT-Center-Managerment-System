using FluentValidation;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.SkillCertificates
{

    public class CreateSkillCertificateViewModelValidator : AbstractValidator<CreateSkillCertificateViewModel>
    {
        public CreateSkillCertificateViewModelValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Nguời dùng không được để trống.");
            RuleFor(x => x.SkillId).NotEmpty().WithMessage("Kĩ năng không được để trống.");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Link không được để trống.")
                .MaximumLength(5000).WithMessage("Link không quá 5000 ký tự."); ;
        }


    }
}
