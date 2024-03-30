using FluentValidation;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Validations.Enrollments
{

    public class UpdateEnrollmentViewModelValidator : AbstractValidator<UpdateEnrollmentViewModel>
    {
        public UpdateEnrollmentViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.");
            RuleFor(x => x.ClassId).NotEmpty().WithMessage("Lớp không được để trống.");
            
        }


    }
}
