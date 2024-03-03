using FluentValidation;
using KidProEdu.Application.ViewModels.RequestViewModels;

namespace KidProEdu.Application.Validations.Requests
{
    public class UpdateRequestViewModelValidator : AbstractValidator<UpdateRequestViewModel>
    {
        public UpdateRequestViewModelValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id yêu cầu không thể để trống");
            RuleFor(x => x.LeaveDate).GreaterThan(DateTime.UtcNow.Date.AddDays(1)).WithMessage("Ngày nghỉ yêu cầu phải sau hiện tại");
            RuleFor(x => x.TeachingDate).GreaterThan(DateTime.UtcNow.Date).WithMessage("Ngày dạy yêu cầu phải sau hiện tại");
        }
    }
}
