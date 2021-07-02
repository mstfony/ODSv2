
using Business.Handlers.DeviceSensors.Commands;
using FluentValidation;

namespace Business.Handlers.DeviceSensors.ValidationRules
{

    public class CreateDeviceSensorValidator : AbstractValidator<CreateDeviceSensorCommand>
    {
        public CreateDeviceSensorValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty();
            RuleFor(x => x.SensorId).NotEmpty();

        }
    }
    public class UpdateDeviceSensorValidator : AbstractValidator<UpdateDeviceSensorCommand>
    {
        public UpdateDeviceSensorValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty();
            RuleFor(x => x.SensorId).NotEmpty();

        }
    }
}