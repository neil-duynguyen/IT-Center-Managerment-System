using FluentValidation;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Semesters
{
    public class CreateSemesterViewModelValidator : AbstractValidator<CreateSemesterViewModel>
    {
        public CreateSemesterViewModelValidator()
        {
            RuleFor(x => x.SemesterName).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");
            RuleFor(x => x.EndDate >= x.StartDate)
                .NotEmpty().WithMessage("Ngày kết thúc không được để trống hoặc trước ngày bắt đầu.");
        }
    }
}
