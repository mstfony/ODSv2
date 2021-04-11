
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Alerts.Queries
{
    public class GetAlertQuery : IRequest<IDataResult<Alert>>
    {
        public int Id { get; set; }

        public class GetAlertQueryHandler : IRequestHandler<GetAlertQuery, IDataResult<Alert>>
        {
            private readonly IAlertRepository _alertRepository;
            private readonly IMediator _mediator;

            public GetAlertQueryHandler(IAlertRepository alertRepository, IMediator mediator)
            {
                _alertRepository = alertRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Alert>> Handle(GetAlertQuery request, CancellationToken cancellationToken)
            {
                var alert = await _alertRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Alert>(alert);
            }
        }
    }
}
