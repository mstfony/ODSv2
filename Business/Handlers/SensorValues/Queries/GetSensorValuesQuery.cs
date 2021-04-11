
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

namespace Business.Handlers.SensorValues.Queries
{

    public class GetSensorValuesQuery : IRequest<IDataResult<IEnumerable<SensorValue>>>
    {
        public class GetSensorValuesQueryHandler : IRequestHandler<GetSensorValuesQuery, IDataResult<IEnumerable<SensorValue>>>
        {
            private readonly ISensorValueRepository _sensorValueRepository;
            private readonly IMediator _mediator;

            public GetSensorValuesQueryHandler(ISensorValueRepository sensorValueRepository, IMediator mediator)
            {
                _sensorValueRepository = sensorValueRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<SensorValue>>> Handle(GetSensorValuesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<SensorValue>>(await _sensorValueRepository.GetListAsync());
            }
        }
    }
}