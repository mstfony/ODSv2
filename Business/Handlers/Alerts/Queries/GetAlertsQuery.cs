
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Alerts.Queries
{

    public class GetAlertsQuery : IRequest<IDataResult<IEnumerable<Alert>>>
    {
        public class GetAlertsQueryHandler : IRequestHandler<GetAlertsQuery, IDataResult<IEnumerable<Alert>>>
        {
            private readonly IAlertRepository _alertRepository;
            private readonly IMediator _mediator;

            public GetAlertsQueryHandler(IAlertRepository alertRepository, IMediator mediator)
            {
                _alertRepository = alertRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Alert>>> Handle(GetAlertsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Alert>>(await _alertRepository.GetListAsync());
            }
        }
    }
}