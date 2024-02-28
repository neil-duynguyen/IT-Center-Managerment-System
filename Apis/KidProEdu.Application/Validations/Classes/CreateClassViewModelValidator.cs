using FluentValidation;
using KidProEdu.Application.ViewModels.ClassViewModels;

namespace KidProEdu.Application.Validations.Classes
{
    public class CreateClassViewModelValidator : AbstractValidator<CreateClassViewModel>
    {
        public CreateClassViewModelValidator()
        {
            RuleFor(x => x.ClassCode).NotNull().NotEmpty().WithMessage("Mã lớp không thể bỏ trống");
            RuleFor(x => x.CourseId).NotNull().NotEmpty().WithMessage("Id khóa học không thể bỏ trống");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Ngày bắt đầu không thể bỏ trống")
                .GreaterThan(DateTime.Now.Date).WithMessage("Ngày bắt đầu không thể trước thời gian hiện tại") ;
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Ngày kết thúc không thể bỏ trống")
                .GreaterThan(x => x.StartDate).WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
            RuleFor(x => x.MaxNumber).NotNull().WithMessage("Số lượng học sinh tối đa không thể bỏ trống")
                .GreaterThan(0).WithMessage("Số lượng học sinh tối đa phải lớn hơn 0");
            RuleFor(x => x.ExpectedNumber).NotNull().WithMessage("Số lượng học sinh kỳ vọng không thể bỏ trống")
                .GreaterThan(0).WithMessage("Số lượng học sinh kỳ vọng phải lớn hơn 0");
        }
    }
}
