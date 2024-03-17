using FluentValidation;
using KidProEdu.Application.ViewModels.ResourceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Resources
{

    public class UpdateResourceViewModelValidator : AbstractValidator<UpdateResourceViewModel>
    {
        public UpdateResourceViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("Khóa học không được để trống.");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Liên kết không được để trống.")
                .MaximumLength(1000).WithMessage("Liên kết không quá 1000 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Tên không quá 1000 ký tự.");
        }


    }
}
