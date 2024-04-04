using FluentValidation;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Course
{
    public class CreateCourseViewModelValidator : AbstractValidator<CreateCourseViewModel>
    {
        public CreateCourseViewModelValidator()
        {
            RuleFor(x => x.CourseCode).NotEmpty().WithMessage("Mã khóa học không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên khoá học không được để trống.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá khoá học không được để trống.");
            RuleFor(x => x.DurationTotal).NotEmpty().WithMessage("Tổng thời lượng slot khoá học không được để trống.");
        }
    }
}
