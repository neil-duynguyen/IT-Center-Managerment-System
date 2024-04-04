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
    public class CreateCourseParentViewModelValidator : AbstractValidator<CreateCourseParentViewModel>
    {
        public CreateCourseParentViewModelValidator()
        {
            RuleFor(x => x.CourseCode).NotEmpty().WithMessage("Mã khóa học không được để trống.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên khoá học không được để trống.");
            RuleFor(x => x.Syllabus).NotEmpty().WithMessage("Giáo trình khoá học không được để trống.");
            RuleFor(x => x.EntryPoint).NotEmpty().WithMessage("Điểm đầu vào khoá học không được để trống.");
            RuleFor(x => x.CourseType).NotEmpty().WithMessage("Loại khoá học không được để trống.");
        }
    }
}
