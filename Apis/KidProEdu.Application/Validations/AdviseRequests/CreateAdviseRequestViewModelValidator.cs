using FluentValidation;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;

namespace KidProEdu.Application.Validations.AdviseRequests
{
    public class CreateAdviseRequestViewModelValidator : AbstractValidator<CreateAdviseRequestViewModel>
    {
        private readonly ICurrentTime _currentTime;
        public CreateAdviseRequestViewModelValidator(ICurrentTime currentTime)
        {
            _currentTime = currentTime;

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không thể bỏ trống");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone không thể bỏ trống");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName không thể bỏ trống");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address không thể bỏ trống");
            RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location không thể bỏ trống");
            //RuleFor(x => x.TestDate.Value.Date).GreaterThanOrEqualTo(_currentTime.GetCurrentTime().Date).WithMessage("Ngày đăng ký làm bài đánh giá đã qua");
        }
    }
}
