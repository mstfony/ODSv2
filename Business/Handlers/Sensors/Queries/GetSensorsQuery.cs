
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

namespace Business.Handlers.Sensors.Queries
{

    public class GetSensorsQuery : IRequest<IDataResult<IEnumerable<Sensor>>>
    {
        public class GetSensorsQueryHandler : IRequestHandler<GetSensorsQuery, IDataResult<IEnumerable<Sensor>>>
        {
            private readonly ISensorRepository _sensorRepository;
            private readonly IMediator _mediator;

            public GetSensorsQueryHandler(ISensorRepository sensorRepository, IMediator mediator)
            {
                _sensorRepository = sensorRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Sensor>>> Handle(GetSensorsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Sensor>>(await _sensorRepository.GetListAsync());
            }
        }
    }
}