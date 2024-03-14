using FluentValidation;
using KidProEdu.Application.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Contracts
{
    public class UpdateContractViewModelValidator : AbstractValidator<UpdateContractViewModel>
    {
        public UpdateContractViewModelValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Ngày bắt đầu không thể bỏ trống");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Ngày kết thúc không thể bỏ trống")
                .GreaterThan(x => x.StartDate).WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
        }
    }
}
