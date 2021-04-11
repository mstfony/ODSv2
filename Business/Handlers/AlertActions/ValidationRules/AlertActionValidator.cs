
using Business.Handlers.AlertActions.Commands;
using FluentValidation;

namespace Business.Handlers.AlertActions.ValidationRules
{

    public class CreateAlertActionValidator : AbstractValidator<CreateAlertActionCommand>
    {
        public CreateAlertActionValidator()
        {
            RuleFor(x => x.SensorSettingId).NotEmpty();
            RuleFor(x => x.AlertId).NotEmpty();

        }
    }
    public class UpdateAlertActionValidator : AbstractValidator<UpdateAlertActionCommand>
    {
        public UpdateAlertActionValidator()
        {
            RuleFor(x => x.SensorSettingId).NotEmpty();
            RuleFor(x => x.AlertId).NotEmpty();

        }
    }
}