
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


namespace Business.Handlers.AlertActionLogs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAlertActionLogCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteAlertActionLogCommandHandler : IRequestHandler<DeleteAlertActionLogCommand, IResult>
        {
            private readonly IAlertActionLogRepository _alertActionLogRepository;
            private readonly IMediator _mediator;

            public DeleteAlertActionLogCommandHandler(IAlertActionLogRepository alertActionLogRepository, IMediator mediator)
            {
                _alertActionLogRepository = alertActionLogRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAlertActionLogCommand request, CancellationToken cancellationToken)
            {
                var alertActionLogToDelete = _alertActionLogRepository.Get(p => p.Id == request.Id);

                _alertActionLogRepository.Delete(alertActionLogToDelete);
                await _alertActionLogRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

