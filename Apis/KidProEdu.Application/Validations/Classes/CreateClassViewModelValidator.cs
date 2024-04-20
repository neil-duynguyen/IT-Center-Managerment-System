using FluentValidation;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ClassViewModels;
using System.Drawing.Text;

namespace KidProEdu.Application.Validations.Classes
{
    public class CreateClassViewModelValidator : AbstractValidator<CreateClassViewModel>
    {
        private readonly ICurrentTime _currentTime;
        public CreateClassViewModelValidator(ICurrentTime currentTime)
        {
            _currentTime = currentTime;

            RuleFor(x => x.ClassCode).NotNull().NotEmpty().WithMessage("Mã lớp không thể bỏ trống");
            RuleFor(x => x.CourseId).NotNull().NotEmpty().WithMessage("Id khóa học không thể bỏ trống");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Ngày bắt đầu không thể bỏ trống")
                .GreaterThan(_currentTime.GetCurrentTime().Date).WithMessage("Ngày bắt đầu không thể trước thời gian hiện tại");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Ngày kết thúc không thể bỏ trống")
                .GreaterThan(x => x.StartDate).WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
            RuleFor(x => x.ExpectedNumber).NotNull().WithMessage("Số lượng học sinh kỳ vọng không thể bỏ trống")
                .GreaterThan(0).WithMessage("Số lượng học sinh kỳ vọng phải lớn hơn 0");
            RuleFor(x => x.MaxNumber).NotNull().WithMessage("Số lượng học sinh tối đa không thể bỏ trống")
                .GreaterThanOrEqualTo(x => x.ExpectedNumber).WithMessage("Số lượng học sinh tối đa phải lớn hơn dự kiến");
        }
    }
}
