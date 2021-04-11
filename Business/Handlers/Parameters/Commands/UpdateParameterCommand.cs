
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Parameters.ValidationRules;


namespace Business.Handlers.Parameters.Commands
{


    public class UpdateParameterCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateParameterCommandHandler : IRequestHandler<UpdateParameterCommand, IResult>
        {
            private readonly IParameterRepository _parameterRepository;
            private readonly IMediator _mediator;

            public UpdateParameterCommandHandler(IParameterRepository parameterRepository, IMediator mediator)
            {
                _parameterRepository = parameterRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateParameterValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateParameterCommand request, CancellationToken cancellationToken)
            {
                var isThereParameterRecord = await _parameterRepository.GetAsync(u => u.Id == request.Id);


                isThereParameterRecord.Name = request.Name;


                _parameterRepository.Update(isThereParameterRecord);
                await _parameterRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

