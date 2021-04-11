
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

namespace Business.Handlers.AlertActions.Queries
{

    public class GetAlertActionsBySsIdQuery : IRequest<IDataResult<IEnumerable<AlertAction>>>
    {
        public int Id { get; set; }
        public class GetAlertActionsBySsIdQueryHandler : IRequestHandler<GetAlertActionsBySsIdQuery, IDataResult<IEnumerable<AlertAction>>>
        {
            private readonly IAlertActionRepository _alertActionRepository;
            private readonly IMediator _mediator;

            public GetAlertActionsBySsIdQueryHandler(IAlertActionRepository alertActionRepository, IMediator mediator)
            {
                _alertActionRepository = alertActionRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
           // [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<AlertAction>>> Handle(GetAlertActionsBySsIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<AlertAction>>(await _alertActionRepository.GetAlertActionListBySensorSettingId(request.Id));
            }
        }
    }
}