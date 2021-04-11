
using Business.Handlers.Sensors.Commands;
using FluentValidation;

namespace Business.Handlers.Sensors.ValidationRules
{

    public class CreateSensorValidator : AbstractValidator<CreateSensorCommand>
    {
        public CreateSensorValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Alias).NotEmpty();

        }
    }
    public class UpdateSensorValidator : AbstractValidator<UpdateSensorCommand>
    {
        public UpdateSensorValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Alias).NotEmpty();

        }
    }
}