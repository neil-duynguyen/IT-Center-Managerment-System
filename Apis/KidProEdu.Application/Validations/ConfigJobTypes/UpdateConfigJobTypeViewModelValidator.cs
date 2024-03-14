using FluentValidation;
using KidProEdu.Application.ViewModels.ConfigJobType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.ConfigJobTypes
{
    public class UpdateConfigJobTypeViewModelValidator : AbstractValidator<UpdateConfigJobTypeViewModel>
    {
        public UpdateConfigJobTypeViewModelValidator()
        {

        }
    }
}
