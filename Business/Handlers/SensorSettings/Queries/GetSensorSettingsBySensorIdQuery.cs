
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

namespace Business.Handlers.SensorSettings.Queries
{

    public class GetSensorSettingsBySensorIdQuery : IRequest<IDataResult<IEnumerable<SensorSetting>>>
    {
        public int Id { get; set; }
        public class GetSensorSettingsBySensorIdQueryHandler : IRequestHandler<GetSensorSettingsBySensorIdQuery, IDataResult<IEnumerable<SensorSetting>>>
        {
            private readonly ISensorSettingRepository _sensorSettingRepository;
            private readonly IMediator _mediator;

            public GetSensorSettingsBySensorIdQueryHandler(ISensorSettingRepository sensorSettingRepository, IMediator mediator)
            {
                _sensorSettingRepository = sensorSettingRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
          //  [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<SensorSetting>>> Handle(GetSensorSettingsBySensorIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<SensorSetting>>(await _sensorSettingRepository.GetSensorSettingListBySensorId(request.Id));
            }
        }
    }
}