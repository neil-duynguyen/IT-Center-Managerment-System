using FluentValidation;
using KidProEdu.Application.ViewModels.TrainingProgramViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.TrainingPrograms
{
    public class UpdateTrainingProgramViewModelValidator : AbstractValidator<UpdateTrainingProgramViewModel>
    {
        public UpdateTrainingProgramViewModelValidator() 
        {
            RuleFor(x => x.TrainingProgramCode).NotEmpty().WithMessage("Mã chương trình không được để trống.")
                .MaximumLength(50).WithMessage("Mã chương trình không quá 50 ký tự.");
            RuleFor(x => x.TrainingProgramName).NotEmpty().WithMessage("Tên không được để trống.")
                    .MaximumLength(255).WithMessage("Tên chương trình không quá 255 ký tự.");
            RuleFor(x => x.Price).NotNull().WithMessage("Giá không được để trống.")
                    .GreaterThanOrEqualTo(0).WithMessage("Giá chương trình không thể là số âm.");
            RuleFor(x => x.TrainingProgramCategoryId).NotEmpty().WithMessage("Id loại chương trình không được để trống.");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id chương trình không được để trống.");
        }
    }
}
