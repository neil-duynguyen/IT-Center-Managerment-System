using FluentValidation;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Equipments
{

    public class LogEquipmentRepairManagementViewModelValidator : AbstractValidator<LogEquipmentRepairManagementViewModel>
    {
        public LogEquipmentRepairManagementViewModelValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người mang thiết bị đi bảo dưỡng không được để trống.");
            
        }


    }
}
