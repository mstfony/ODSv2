
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


namespace Business.Handlers.AlertActions.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAlertActionCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteAlertActionCommandHandler : IRequestHandler<DeleteAlertActionCommand, IResult>
        {
            private readonly IAlertActionRepository _alertActionRepository;
            private readonly IMediator _mediator;

            public DeleteAlertActionCommandHandler(IAlertActionRepository alertActionRepository, IMediator mediator)
            {
                _alertActionRepository = alertActionRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAlertActionCommand request, CancellationToken cancellationToken)
            {
                var alertActionToDelete = _alertActionRepository.Get(p => p.Id == request.Id);

                _alertActionRepository.Delete(alertActionToDelete);
                await _alertActionRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

