using FluentValidation;
using KidProEdu.Application.ViewModels.DivisionUserAccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.DivisionUserAccounts
{

    public class UpdateDivisionUserAccountViewModelValidator : AbstractValidator<UpdateDivisionUserAccountViewModel>
    {
        public UpdateDivisionUserAccountViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người dùng không được để trống.");
            RuleFor(x => x.DivisionId).NotEmpty().WithMessage("Bộ phận không được để trống.");
        }


    }
}
