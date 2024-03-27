using FluentValidation;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Exams
{

    public class CreateExamViewModelValidator : AbstractValidator<CreateExamViewModel2>
    {
        public CreateExamViewModelValidator()
        {
            RuleFor(x => x.TestName).NotEmpty().WithMessage("Tên bài kiểm tra không được để trống.")
                .MaximumLength(100).WithMessage("Địa chỉ không quá 100 ký tự.");
            RuleFor(x => x.TestName).NotEmpty().WithMessage("Tên bài kiểm tra không được để trống.")
                .MaximumLength(100).WithMessage("Địa chỉ không quá 100 ký tự.");
            RuleFor(x => x.TestDate).NotEmpty().WithMessage("Thời gian kiểm tra không được để trống.");
            RuleFor(x => x.TestDuration)
                .NotEmpty().WithMessage("Thời lượng kiểm tra không được để trống.")
                .GreaterThan(0).WithMessage("Thời lượng kiểm tra phải lớn hơn 0.");
            RuleFor(x => x.TestType).NotEmpty().WithMessage("Loại kiểm tra không được để trống.");
        }


    }
}
