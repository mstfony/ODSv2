
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Parameters.ValidationRules;

namespace Business.Handlers.Parameters.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateParameterCommand : IRequest<IResult>
    {

        public string Name { get; set; }


        public class CreateParameterCommandHandler : IRequestHandler<CreateParameterCommand, IResult>
        {
            private readonly IParameterRepository _parameterRepository;
            private readonly IMediator _mediator;
            public CreateParameterCommandHandler(IParameterRepository parameterRepository, IMediator mediator)
            {
                _parameterRepository = parameterRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateParameterValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateParameterCommand request, CancellationToken cancellationToken)
            {
                var isThereParameterRecord = _parameterRepository.Query().Any(u => u.Name == request.Name);

                if (isThereParameterRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedParameter = new Parameter
                {
                    Name = request.Name,

                };

                _parameterRepository.Add(addedParameter);
                await _parameterRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}