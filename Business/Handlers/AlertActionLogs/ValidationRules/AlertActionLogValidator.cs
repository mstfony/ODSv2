
using Business.Handlers.AlertActionLogs.Commands;
using FluentValidation;

namespace Business.Handlers.AlertActionLogs.ValidationRules
{

    public class CreateAlertActionLogValidator : AbstractValidator<CreateAlertActionLogCommand>
    {
        public CreateAlertActionLogValidator()
        {
            RuleFor(x => x.AlertActionUserId).NotEmpty();
            RuleFor(x => x.DateTime).NotEmpty();

        }
    }
    public class UpdateAlertActionLogValidator : AbstractValidator<UpdateAlertActionLogCommand>
    {
        public UpdateAlertActionLogValidator()
        {
            RuleFor(x => x.AlertActionUserId).NotEmpty();
            RuleFor(x => x.DateTime).NotEmpty();

        }
    }
}