
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


namespace Business.Handlers.Alerts.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAlertCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteAlertCommandHandler : IRequestHandler<DeleteAlertCommand, IResult>
        {
            private readonly IAlertRepository _alertRepository;
            private readonly IMediator _mediator;

            public DeleteAlertCommandHandler(IAlertRepository alertRepository, IMediator mediator)
            {
                _alertRepository = alertRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
            {
                var alertToDelete = _alertRepository.Get(p => p.Id == request.Id);

                _alertRepository.Delete(alertToDelete);
                await _alertRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

