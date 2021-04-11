
using Business.Handlers.Parameters.Commands;
using FluentValidation;

namespace Business.Handlers.Parameters.ValidationRules
{

    public class CreateParameterValidator : AbstractValidator<CreateParameterCommand>
    {
        public CreateParameterValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
    public class UpdateParameterValidator : AbstractValidator<UpdateParameterCommand>
    {
        public UpdateParameterValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}