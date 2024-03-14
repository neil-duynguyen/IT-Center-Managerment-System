using FluentValidation;
using KidProEdu.Application.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Contracts
{
    public class CreateContractViewModelValidator : AbstractValidator<CreateContractViewModel>
    {
        public CreateContractViewModelValidator()
        {
            RuleFor(x => x.ContractCode).NotEmpty().WithMessage("Mã hợp đồng không thể để trống");
        }
    }
}
