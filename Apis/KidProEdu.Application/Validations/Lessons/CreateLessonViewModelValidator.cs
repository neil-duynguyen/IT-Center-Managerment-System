using FluentValidation;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Lessons
{

    public class CreateLessonViewModelValidator : AbstractValidator<CreateLessonViewModel>
    {
        public CreateLessonViewModelValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("Khóa học không được để trống.");
            RuleFor(x => x.LessonNumber).NotEmpty().WithMessage("Số bài học không được để trống.");
            RuleFor(x => x.Prerequisites).NotEmpty().WithMessage("Điều kiện tiên quyết không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(500).WithMessage("Mô tả không quá 500 ký tự.");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Khoảng thời gian không được để trống.");
        }


    }
}
