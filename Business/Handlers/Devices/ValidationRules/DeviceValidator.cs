
using Business.Handlers.Devices.Commands;
using FluentValidation;

namespace Business.Handlers.Devices.ValidationRules
{

    public class CreateDeviceValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.Describe).NotEmpty();

        }
    }
    public class UpdateDeviceValidator : AbstractValidator<UpdateDeviceCommand>
    {
        public UpdateDeviceValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.Describe).NotEmpty();

        }
    }
}