using FluentValidation;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.CategoryEquipments
{

    public class UpdateCategoryEquipmentViewModelValidator : AbstractValidator<UpdateCategoryEquipmentViewModel>
    {
        public UpdateCategoryEquipmentViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(500).WithMessage("Mô tả không quá 500 ký tự.");
        }


    }
}
