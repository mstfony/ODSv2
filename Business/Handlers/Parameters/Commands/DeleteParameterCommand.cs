
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Parameters.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteParameterCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteParameterCommandHandler : IRequestHandler<DeleteParameterCommand, IResult>
        {
            private readonly IParameterRepository _parameterRepository;
            private readonly IMediator _mediator;

            public DeleteParameterCommandHandler(IParameterRepository parameterRepository, IMediator mediator)
            {
                _parameterRepository = parameterRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteParameterCommand request, CancellationToken cancellationToken)
            {
                var parameterToDelete = _parameterRepository.Get(p => p.Id == request.Id);

                _parameterRepository.Delete(parameterToDelete);
                await _parameterRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

