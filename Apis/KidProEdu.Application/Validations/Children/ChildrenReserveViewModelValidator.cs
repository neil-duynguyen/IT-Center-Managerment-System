using FluentValidation;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Children
{

    public class ChildrenReserveViewModelValidator : AbstractValidator<ChildrenReserveViewModel>
    {
        public ChildrenReserveViewModelValidator()
        {
            RuleFor(x => x.ChildrenProfileId).NotEmpty().WithMessage("Học sinh không được để trống.");
        }
    }
}
