using FluentValidation;
using KidProEdu.Application.ViewModels.TagViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Validations.Tags
{
    public class CreateTagValidator : AbstractValidator<CreateTagViewModel>
    {
        public CreateTagValidator()
        {
            RuleFor(x => x.TagName).NotEmpty().WithMessage("Tên không được để trống.")
                .Must(IsIdentityName).WithMessage("Tên đã được sử dụng.").MaximumLength(50)
                .WithMessage("Tên không quá 50 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(500)
                .WithMessage("Mô tả không quá 500 ký tự.");                 
        }

        private bool IsIdentityName(string name)
        {
            var _context = new AppDbContext();
            var tag = _context.Tags.FirstOrDefault(x => x.TagName.ToLower().Equals(name.ToLower()));
            if (tag == null)
            {
                return true;
            }
            return false;
        }


    }
}
