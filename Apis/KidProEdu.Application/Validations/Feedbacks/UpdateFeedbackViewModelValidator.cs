using FluentValidation;
using KidProEdu.Application.ViewModels.FeedBackViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Feedbacks
{

    public class UpdateFeedbackViewModelValidator : AbstractValidator<UpdateFeedBackViewModel>
    {
        public UpdateFeedbackViewModelValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Nguời dùng không được để trống.");
            RuleFor(x => x.RecipientId).NotEmpty().WithMessage("Lớp không được để trống.");
            RuleFor(x => x.ClassId).NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");
            RuleFor(x => x.Messages).NotEmpty().WithMessage("Ngày kết thúc không được để trống.")
                .MaximumLength(1000).WithMessage("Nội dung không quá 1000 ký tự.");
            RuleFor(x => x.Stars).NotEmpty().WithMessage("Số sao không được để trống.")
                .MaximumLength(100).WithMessage("Số sao không quá 100 ký tự.");
        }


    }
}
