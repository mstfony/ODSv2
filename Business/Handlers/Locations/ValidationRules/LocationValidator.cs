
using Business.Handlers.Locations.Commands;
using FluentValidation;

namespace Business.Handlers.Locations.ValidationRules
{

    public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
    public class UpdateLocationValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}