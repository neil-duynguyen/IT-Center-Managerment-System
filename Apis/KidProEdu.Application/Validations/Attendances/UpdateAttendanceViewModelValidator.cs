using FluentValidation;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Attendances
{

    public class UpdateAttendanceViewModelValidator : AbstractValidator<UpdateAttendanceViewModel>
    {
        public UpdateAttendanceViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.StatusAttendance).NotEmpty().WithMessage("Trạng thái không được để trống.");
        }


    }
}
