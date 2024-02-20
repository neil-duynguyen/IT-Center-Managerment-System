using FluentValidation;
using KidProEdu.Application.ViewModels.RatingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Ratings
{

    public class UpdateRatingViewModelValidator : AbstractValidator<UpdateRatingViewModel>
    {
        public UpdateRatingViewModelValidator()
        {
            RuleFor(x => x.StarNumber).NotEmpty().WithMessage("Số sao không được để trống.");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Bình luận không được để trống.");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
        }


    }
}
