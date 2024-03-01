using FluentValidation;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Documents
{

    public class CreateDocumentViewModelValidator : AbstractValidator<CreateDocumentViewModel>
    {
        public CreateDocumentViewModelValidator()
        {
            RuleFor(x => x.LessonId).NotEmpty().WithMessage("Bài học không được để trống.");
            RuleFor(x => x.ClassId).NotEmpty().WithMessage("Lớp học không được để trống.");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Liên kết không được để trống.")
                .MaximumLength(500).WithMessage("Tên không quá 500 ký tự.");
        }


    }
}
