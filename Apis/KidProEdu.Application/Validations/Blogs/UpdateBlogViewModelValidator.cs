using FluentValidation;
using KidProEdu.Application.ViewModels.BlogViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Blogs
{

    public class UpdateBlogViewModelValidator : AbstractValidator<UpdateBlogViewModel>
    {
        public UpdateBlogViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Người tạo không được để trống.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung không được để trống.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề không được để trống.");
        }


    }
}
