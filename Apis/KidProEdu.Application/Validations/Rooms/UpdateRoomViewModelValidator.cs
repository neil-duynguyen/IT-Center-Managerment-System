using FluentValidation;
using KidProEdu.Application.ViewModels.RoomViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Rooms
{

    public class UpdateRoomViewModelValidator : AbstractValidator<UpdateRoomViewModel>
    {
        public UpdateRoomViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Trạng thái không được để trống.")
                .IsInEnum().WithMessage("Trạng thái không hợp lệ.");
        }


    }
}
