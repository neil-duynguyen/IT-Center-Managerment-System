using FluentValidation;
using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.ChildrenAnswers
{

    public class UpdateChildrenAnswerViewModelValidator : AbstractValidator<UpdateChildrenAnswerViewModel>
    {
        public UpdateChildrenAnswerViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Nguời dùng không được để trống.");
            RuleFor(x => x.ChildrenProfileId).NotEmpty().WithMessage("Nguời dùng không được để trống.");
            RuleFor(x => x.ExamId).NotEmpty().WithMessage("Bài kiểm tra không được để trống.");
            RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Câu hỏi không được để trống.");
            RuleFor(x => x.Answer).NotEmpty().WithMessage("Câu trả lời không được để trống.");
            RuleFor(x => x.ScorePerQuestion)
                .GreaterThanOrEqualTo(0).WithMessage("Điểm phải lớn hơn hoặc bằng 0.");
        }


    }
}
