
using Business.Handlers.Settings.Commands;
using FluentValidation;

namespace Business.Handlers.Settings.ValidationRules
{

    public class CreateSettingValidator : AbstractValidator<CreateSettingCommand>
    {
        public CreateSettingValidator()
        {
            RuleFor(x => x.ParameterId).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();

        }
    }
    public class UpdateSettingValidator : AbstractValidator<UpdateSettingCommand>
    {
        public UpdateSettingValidator()
        {
            RuleFor(x => x.ParameterId).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();

        }
    }
}