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

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.");

            RuleFor(x => x.Duration).NotEmpty().WithMessage("Khoảng thời gian học không được để trống.").GreaterThan(0).WithMessage("Khoảng thời gian học không được bé hơn hoặc bằng 0.");
            
        }
    }
}
