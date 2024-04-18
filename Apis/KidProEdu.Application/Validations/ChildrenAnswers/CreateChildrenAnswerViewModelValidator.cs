using FluentValidation;
using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.ChildrenAnswers
{

    public class CreateChildrenAnswerViewModelValidator : AbstractValidator<CreateChildrenAnswerViewModel>
    {
        public CreateChildrenAnswerViewModelValidator()
        {
            RuleFor(x => x.ChildrenProfileId).NotEmpty().WithMessage("Nguời dùng không được để trống.");
            RuleFor(x => x.ExamId).NotEmpty().WithMessage("Bài kiểm tra không được để trống.");
            //RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Câu hỏi không được để trống.");
            RuleFor(x => x.ScorePerQuestion)
                .GreaterThanOrEqualTo(0).WithMessage("Điểm phải lớn hơn hoặc bằng 0.");
        }


    }
}
