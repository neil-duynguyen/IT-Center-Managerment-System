using FluentValidation;
using KidProEdu.Application.ViewModels.ScheduleViewModels;

namespace KidProEdu.Application.Validations.Schedules
{
    public class UpdateScheduleViewModelValidator : AbstractValidator<UpdateScheduleViewModel>
    {
        public UpdateScheduleViewModelValidator() 
        {
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
        }
    }
}
