
using Business.Handlers.AlertActionUsers.Commands;
using FluentValidation;

namespace Business.Handlers.AlertActionUsers.ValidationRules
{

    public class CreateAlertActionUserValidator : AbstractValidator<CreateAlertActionUserCommand>
    {
        public CreateAlertActionUserValidator()
        {
            RuleFor(x => x.AlertActionId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();

        }
    }
    public class UpdateAlertActionUserValidator : AbstractValidator<UpdateAlertActionUserCommand>
    {
        public UpdateAlertActionUserValidator()
        {
            RuleFor(x => x.AlertActionId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();

        }
    }
}