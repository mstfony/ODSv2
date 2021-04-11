
using Business.Handlers.Alerts.Commands;
using FluentValidation;

namespace Business.Handlers.Alerts.ValidationRules
{

    public class CreateAlertValidator : AbstractValidator<CreateAlertCommand>
    {
        public CreateAlertValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
    public class UpdateAlertValidator : AbstractValidator<UpdateAlertCommand>
    {
        public UpdateAlertValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}