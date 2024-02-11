using FluentValidation;
using KidProEdu.Application.ViewModels.BlogTagViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.BlogTags
{

    public class UpdateBlogTagViewModelValidator : AbstractValidator<UpdateBlogTagViewModel>
    {
        public UpdateBlogTagViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.TagId).NotEmpty().WithMessage("Tag không được để trống.");
            RuleFor(x => x.BlogId).NotEmpty().WithMessage("Bài viết không được để trống.");
        }


    }
}
