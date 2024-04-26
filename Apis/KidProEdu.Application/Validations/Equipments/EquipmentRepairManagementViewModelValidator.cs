using FluentValidation;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Equipments
{

    public class EquipmentRepairManagementViewModelValidator : AbstractValidator<EquipmentRepairManagementViewModel>
    {
        public EquipmentRepairManagementViewModelValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người mang thiết bị đi bảo dưỡng không được để trống.");
            
        }


    }
}
