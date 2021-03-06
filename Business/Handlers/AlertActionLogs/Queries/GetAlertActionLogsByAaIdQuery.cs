
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

namespace Business.Handlers.AlertActionLogs.Queries
{

    public class GetAlertActionLogsByAaIdQuery : IRequest<IDataResult<IEnumerable<AlertActionLog>>>
    {
        public int id { get; set; }
        public class GetAlertActionLogsByAaIdQueryHandler : IRequestHandler<GetAlertActionLogsByAaIdQuery, IDataResult<IEnumerable<AlertActionLog>>>
        {
            private readonly IAlertActionLogRepository _alertActionLogRepository;
            private readonly IMediator _mediator;

            public GetAlertActionLogsByAaIdQueryHandler(IAlertActionLogRepository alertActionLogRepository, IMediator mediator)
            {
                _alertActionLogRepository = alertActionLogRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
         //   [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<AlertActionLog>>> Handle(GetAlertActionLogsByAaIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<AlertActionLog>>(await _alertActionLogRepository.GetAlertActionLogByAaId(request.id));
            }
        }
    }
}