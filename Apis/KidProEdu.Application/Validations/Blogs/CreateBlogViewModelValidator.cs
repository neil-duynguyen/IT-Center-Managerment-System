using FluentValidation;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Blogs
{

    public class CreateBlogViewModelValidator : AbstractValidator<CreateBlogViewModel>
    {
        public CreateBlogViewModelValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Người dùng không được để trống.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung không được để trống.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề không được để trống.");
        }


    }
}
