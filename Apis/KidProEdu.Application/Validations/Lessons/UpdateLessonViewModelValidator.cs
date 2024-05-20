using FluentValidation;
using KidProEdu.Application.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Lessons
{

    public class UpdateLessonViewModelValidator : AbstractValidator<UpdateLessonViewModel>
    {
        public UpdateLessonViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");

            RuleFor(x => x.CourseId).NotEmpty().WithMessage("Khóa học không được để trống.");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.");

            RuleFor(x => x.Duration).NotEmpty().WithMessage("Khoảng thời gian không được để trống.");
        }


    }
}
