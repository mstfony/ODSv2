
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

namespace Business.Handlers.DeviceSensors.Queries
{

    public class GetDeviceSensorsQuery : IRequest<IDataResult<IEnumerable<DeviceSensor>>>
    {
        public class GetDeviceSensorsQueryHandler : IRequestHandler<GetDeviceSensorsQuery, IDataResult<IEnumerable<DeviceSensor>>>
        {
            private readonly IDeviceSensorRepository _deviceSensorRepository;
            private readonly IMediator _mediator;

            public GetDeviceSensorsQueryHandler(IDeviceSensorRepository deviceSensorRepository, IMediator mediator)
            {
                _deviceSensorRepository = deviceSensorRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<DeviceSensor>>> Handle(GetDeviceSensorsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<DeviceSensor>>(await _deviceSensorRepository.GetListAsync());
            }
        }
    }
}