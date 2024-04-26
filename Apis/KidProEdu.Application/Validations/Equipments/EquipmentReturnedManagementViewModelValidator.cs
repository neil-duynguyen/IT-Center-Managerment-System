using FluentValidation;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Equipments
{

    public class EquipmentReturnedManagementViewModelValidator : AbstractValidator<EquipmentReturnedManagementViewModel>
    {
        public EquipmentReturnedManagementViewModelValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người trả thiết bị không được để trống.");

        }


    }
}
