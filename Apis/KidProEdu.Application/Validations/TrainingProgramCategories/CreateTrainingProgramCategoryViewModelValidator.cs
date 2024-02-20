﻿using FluentValidation;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.TrainingProgramCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.TrainingProgramCategories
{

    public class CreateTrainingProgramCategoryViewModelValidator : AbstractValidator<CreateTrainingProgramCategoryViewModel>
    {
        public CreateTrainingProgramCategoryViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(255).WithMessage("Tên không quá 255 ký tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không quá 1000 ký tự.");
            RuleFor(x => x.LearningAge).NotEmpty().WithMessage("Tuổi không được để trống.")
                .MaximumLength(50).WithMessage("Tuổi không quá 50 ký tự.");
        }


    }
}
