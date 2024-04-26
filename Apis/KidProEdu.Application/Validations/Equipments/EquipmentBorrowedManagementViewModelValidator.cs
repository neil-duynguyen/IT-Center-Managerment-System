using FluentValidation;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Equipments
{

    public class EquipmentBorrowedManagementViewModelValidator : AbstractValidator<EquipmentBorrowedManagementViewModel>
    {
        public EquipmentBorrowedManagementViewModelValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người mượn thiết bị không được để trống.");
            RuleFor(x => x.ReturnedDealine).NotEmpty().WithMessage("Ngày mượn không được để trống.")
                .Must(BeValidReturnedDealine).WithMessage("Ngày trả phải lớn hơn hoặc bằng ngày hiện tại.");    
   
        }

        private bool BeValidReturnedDealine(DateTime? returnedDealine)
        {
            return !returnedDealine.HasValue || returnedDealine.Value.Date >= DateTime.Today;
        }
    }
}
