using FluentValidation;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ExamViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Exams
{
    public class CreateExamFinalPracticeViewModelValidator : AbstractValidator<CreateExamFinalPracticeViewModel>
    {
        public CreateExamFinalPracticeViewModelValidator(DateTime dateTime)
        {
            RuleFor(x => x.TestName).NotEmpty().WithMessage("Tên bài kiểm tra không được để trống.")
                .MaximumLength(100).WithMessage("Địa chỉ không quá 100 ký tự.");
            RuleFor(x => x.TestDate).NotEmpty().WithMessage("Thời gian kiểm tra không được để trống.")
                .GreaterThan(dateTime.Date).WithMessage("Ngày bắt đầu không thể trước thời gian hiện tại");
            RuleFor(x => x.TestDuration)
                .NotEmpty().WithMessage("Thời lượng kiểm tra không được để trống.")
                .GreaterThan(0).WithMessage("Thời lượng kiểm tra phải lớn hơn 0.");
        }
    }
}