using FluentValidation;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Equipments
{

    public class CreateEquipmentViewModelValidator : AbstractValidator<CreateEquipmentViewModel>
    {
        public CreateEquipmentViewModelValidator()
        {
            RuleFor(x => x.CategoryEquipmentId).NotEmpty().WithMessage("Danh mục thiết bị không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Giá phải lớn hơn hoặc bằng 0.");
            //RuleFor(x => x.Status).NotEmpty().WithMessage("Trạng thái không được để trống.")
              //  .IsInEnum().WithMessage("Trạng thái không hợp lệ.");
            RuleFor(x => x.WarrantyPeriod).NotEmpty().WithMessage("Thời hạn bảo hành không được để trống.")
                .MaximumLength(50).WithMessage("Thời hạn bảo hành không quá 50 ký tự.");
            RuleFor(x => x.PurchaseDate).NotEmpty().WithMessage("Ngày mua không được để trống.")
                .Must(BeValidPurchaseDate).WithMessage("Ngày mua phải nhỏ hơn hoặc bằng ngày hiện tại.");
        }

        private bool BeValidPurchaseDate(DateTime? purchaseDate)
        {
            return !purchaseDate.HasValue || purchaseDate.Value.Date <= DateTime.Today;
        }

    }

}