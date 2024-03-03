using FluentValidation;
using KidProEdu.Application.ViewModels.RequestUserAccountViewModels;

namespace KidProEdu.Application.Validations.RequestUserAccounts
{
    public class CreateRequestUserAccountViewModelValidator : AbstractValidator<CreateRequestUserAccountViewModel>
    {
        public CreateRequestUserAccountViewModelValidator()
        {
            RuleFor(x => x.RecieverIds)
                .NotNull().WithMessage("Mã id người nhận không thể bỏ trống");
            RuleFor(x => x.RequestId)
                .NotNull().NotEmpty().WithMessage("Mã id yêu cầu không thể bỏ trống");
        }
    }
}
