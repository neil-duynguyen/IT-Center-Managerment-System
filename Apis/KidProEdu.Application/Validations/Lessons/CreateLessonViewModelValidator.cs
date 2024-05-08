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

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.");

            RuleFor(x => x.Duration).NotEmpty().WithMessage("Khoảng thời gian không được để trống.");

            RuleFor(x => x.TypeOfPractice).NotEmpty().WithMessage("Phương thức thực hành không được để trống.");

            RuleFor(x => x.GroupSize).NotEmpty().WithMessage("Số lượng thành viên trong nhóm không được để trống.")
                .LessThanOrEqualTo(0).WithMessage("Số lượng thành viên trong nhóm không được nhỏ hơn hoặc bằng 0");
            
        }
    }
}
