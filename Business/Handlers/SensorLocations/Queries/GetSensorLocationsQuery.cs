
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

namespace Business.Handlers.SensorLocations.Queries
{

    public class GetSensorLocationsQuery : IRequest<IDataResult<IEnumerable<SensorLocation>>>
    {
        public class GetSensorLocationsQueryHandler : IRequestHandler<GetSensorLocationsQuery, IDataResult<IEnumerable<SensorLocation>>>
        {
            private readonly ISensorLocationRepository _sensorLocationRepository;
            private readonly IMediator _mediator;

            public GetSensorLocationsQueryHandler(ISensorLocationRepository sensorLocationRepository, IMediator mediator)
            {
                _sensorLocationRepository = sensorLocationRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<SensorLocation>>> Handle(GetSensorLocationsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<SensorLocation>>(await _sensorLocationRepository.GetListAsync());
            }
        }
    }
}