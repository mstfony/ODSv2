
using Business.Handlers.SensorSettings.Commands;
using FluentValidation;

namespace Business.Handlers.SensorSettings.ValidationRules
{

    public class CreateSensorSettingValidator : AbstractValidator<CreateSensorSettingCommand>
    {
        public CreateSensorSettingValidator()
        {
            RuleFor(x => x.SensorId).NotEmpty();
            RuleFor(x => x.SettingId).NotEmpty();

        }
    }
    public class UpdateSensorSettingValidator : AbstractValidator<UpdateSensorSettingCommand>
    {
        public UpdateSensorSettingValidator()
        {
            RuleFor(x => x.SensorId).NotEmpty();
            RuleFor(x => x.SettingId).NotEmpty();

        }
    }
}