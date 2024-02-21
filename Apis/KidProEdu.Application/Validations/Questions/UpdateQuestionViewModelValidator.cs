using FluentValidation;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Questions
{
    public class UpdateQuestionViewModelValidator : AbstractValidator<UpdateQuestionViewModel>
    {
        public UpdateQuestionViewModelValidator()
        {
            RuleFor(x => x.LessionId).NotEmpty().WithMessage("Id bài học không thể bỏ trống");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title câu hỏi không thể bỏ trống");
            RuleFor(x => x.Level)
                .GreaterThan(0).WithMessage("Cấp độ phải lớn hơn 0")
                .LessThanOrEqualTo(3).WithMessage("Cấp độ phải nhỏ hơn 3");
        }
    }
}
