using FluentValidation;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Attendances
{

    public class CreateAttendanceViewModelValidator : AbstractValidator<CreateAttendanceViewModel>
    {
        public CreateAttendanceViewModelValidator()
        {
            RuleFor(x => x.ChildrenProfileId).NotEmpty().WithMessage("Id người dùng không được để trống.");
            RuleFor(x => x.ScheduleId).NotEmpty().WithMessage("Id lịch học không được để trống.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Ngày không được để trống.");
            RuleFor(x => x.StatusAttendance).NotEmpty().WithMessage("Trạng thái không được để trống.");

        }


    }
}
