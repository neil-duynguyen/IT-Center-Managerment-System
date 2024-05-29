
using FluentValidation;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.CategoryEquipments
{

    public class BorrowForGoHomeCategoryEquipmentViewModelValidator : AbstractValidator<BorrowForGoHomeCategoryEquipmentViewModel>
    {
        public BorrowForGoHomeCategoryEquipmentViewModelValidator()
        {
            RuleFor(x => x.CategoryEquipmentId).NotEmpty().WithMessage("Thiết bị không được để trống.");
            RuleFor(x => x.UserAccountId).NotEmpty().WithMessage("Người mượn không được để trống.");
            RuleFor(x => x.Quantity).NotNull().WithMessage("Số lượng không được để trống.")
                .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0.");
            RuleFor(x => x.ReturnedDealine)
                .NotEmpty().WithMessage("Ngày trả không được để trống.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Ngày trả phải lớn hơn hoặc bằng ngày hiện tại.");
        }


    }
}
