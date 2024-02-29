using FluentValidation;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Notifications
{

    public class CreateNotificationViewModelValidator : AbstractValidator<CreateNotificationViewModel>
    {
        public CreateNotificationViewModelValidator()
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage("Nội dung không được để trống.")
                .MaximumLength(10000).WithMessage("Thông báo không quá 10000 ký tự.");

        }


    }
}
