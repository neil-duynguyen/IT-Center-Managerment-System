using FluentValidation;
using KidProEdu.Application.ViewModels.DivisionUserAccountViewModels;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.DivisionUserAccounts
{

    public class CreateDivisionUserAccountViewModelValidator : AbstractValidator<CreateDivisionUserAccountViewModel>
    {
        public CreateDivisionUserAccountViewModelValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người dùng không được để trống.");
            RuleFor(x => x.DivisionId).NotEmpty().WithMessage("Bộ phận không được để trống.");
        }


    }
}
