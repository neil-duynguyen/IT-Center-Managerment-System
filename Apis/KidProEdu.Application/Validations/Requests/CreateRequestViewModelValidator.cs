using FluentValidation;
using KidProEdu.Application.ViewModels.RequestViewModels;

namespace KidProEdu.Application.Validations.Requests
{
    public class CreateRequestViewModelValidator : AbstractValidator<CreateRequestViewModel>
    {
        public CreateRequestViewModelValidator()
        {
            //RuleFor(x => x.UserId).NotEmpty().WithMessage("Id người gửi không thể để trống");
            RuleFor(x => x.RequestType).NotEmpty().WithMessage("Loại yêu cầu không thể để trống");
            RuleFor(x => x.LeaveDate).GreaterThan(DateTime.UtcNow.Date.AddDays(1)).WithMessage("Ngày nghỉ yêu cầu phải sau hiện tại");
            RuleFor(x => x.TeachingDate).GreaterThan(DateTime.UtcNow.Date.AddDays(1)).WithMessage("Ngày dạy yêu cầu phải sau hiện tại");
        }
    }
}
