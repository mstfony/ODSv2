
using Business.Handlers.SensorLocations.Commands;
using FluentValidation;

namespace Business.Handlers.SensorLocations.ValidationRules
{

    public class CreateSensorLocationValidator : AbstractValidator<CreateSensorLocationCommand>
    {
        public CreateSensorLocationValidator()
        {
            RuleFor(x => x.SensorId).NotEmpty();
            RuleFor(x => x.LocationId).NotEmpty();

        }
    }
    public class UpdateSensorLocationValidator : AbstractValidator<UpdateSensorLocationCommand>
    {
        public UpdateSensorLocationValidator()
        {
            RuleFor(x => x.SensorId).NotEmpty();
            RuleFor(x => x.LocationId).NotEmpty();

        }
    }
}