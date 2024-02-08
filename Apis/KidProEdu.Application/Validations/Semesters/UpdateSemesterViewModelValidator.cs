using FluentValidation;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Semesters
{
    public class UpdateSemesterViewModelValidator : AbstractValidator<UpdateSemesterViewModel>
    {
        public UpdateSemesterViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.SemesterName).NotEmpty().WithMessage("Tên không được bỏ trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Ngày bắt đầu không được bỏ trống.");
            RuleFor(x => x.EndDate >= x.EndDate)
                .NotEmpty().WithMessage("Ngày kết thúc không được bỏ trống hoặc trước ngày bắt đầu.");
        }
    }
}
