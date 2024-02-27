using FluentValidation;
using KidProEdu.Application.ViewModels.RequestViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Requests
{
    public class CreateRequestViewModelValidator : AbstractValidator<CreateRequestViewModel>
    {
        public CreateRequestViewModelValidator()
        {
            //RuleFor(x => x.UserId).NotEmpty().WithMessage("Id người gửi không thể để trống");
            RuleFor(x => x.RequestType).NotEmpty().WithMessage("Loại yêu cầu không thể để trống");
            RuleFor(x => x.LeaveDate).NotEmpty().WithMessage("Ngày yêu cầu nghỉ không thể để trống")
                .GreaterThan(DateTime.UtcNow.Date).WithMessage("Ngày nghỉ yêu cầu phải sau hiện tại");
        }
    }
}
