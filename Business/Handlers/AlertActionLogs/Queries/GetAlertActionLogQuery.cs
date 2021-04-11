
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.AlertActionLogs.Queries
{
    public class GetAlertActionLogQuery : IRequest<IDataResult<AlertActionLog>>
    {
        public int Id { get; set; }

        public class GetAlertActionLogQueryHandler : IRequestHandler<GetAlertActionLogQuery, IDataResult<AlertActionLog>>
        {
            private readonly IAlertActionLogRepository _alertActionLogRepository;
            private readonly IMediator _mediator;

            public GetAlertActionLogQueryHandler(IAlertActionLogRepository alertActionLogRepository, IMediator mediator)
            {
                _alertActionLogRepository = alertActionLogRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<AlertActionLog>> Handle(GetAlertActionLogQuery request, CancellationToken cancellationToken)
            {
                var alertActionLog = await _alertActionLogRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<AlertActionLog>(alertActionLog);
            }
        }
    }
}
