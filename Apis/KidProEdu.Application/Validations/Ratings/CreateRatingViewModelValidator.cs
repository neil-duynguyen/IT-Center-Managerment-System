using FluentValidation;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Ratings
{

    public class CreateRatingViewModelValidator : AbstractValidator<CreateRatingViewModel>
    {
        public CreateRatingViewModelValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("Khóa học không được để trống.");
            RuleFor(x => x.StarNumber).NotEmpty().WithMessage("Số sao không được để trống.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Người dùng không được để trống.");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Bình luận không được để trống.");
        }


    }
}
