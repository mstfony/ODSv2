
using Business.Handlers.SensorValues.Commands;
using FluentValidation;

namespace Business.Handlers.SensorValues.ValidationRules
{

    public class CreateSensorValueValidator : AbstractValidator<CreateSensorValueCommand>
    {
        public CreateSensorValueValidator()
        {
           // RuleFor(x => x.SensorId).NotEmpty();
          //  RuleFor(x => x.Value).NotEmpty();
          //  RuleFor(x => x.DateTime).NotEmpty();

        }
    }
    public class UpdateSensorValueValidator : AbstractValidator<UpdateSensorValueCommand>
    {
        public UpdateSensorValueValidator()
        {
          //  RuleFor(x => x.SensorId).NotEmpty();
          //  RuleFor(x => x.Value).NotEmpty();
         //   RuleFor(x => x.DateTime).NotEmpty();

        }
    }
}