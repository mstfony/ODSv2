
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

    public class GetSensorSettingsQuery : IRequest<IDataResult<IEnumerable<SensorSetting>>>
    {
        public class GetSensorSettingsQueryHandler : IRequestHandler<GetSensorSettingsQuery, IDataResult<IEnumerable<SensorSetting>>>
        {
            private readonly ISensorSettingRepository _sensorSettingRepository;
            private readonly IMediator _mediator;

            public GetSensorSettingsQueryHandler(ISensorSettingRepository sensorSettingRepository, IMediator mediator)
            {
                _sensorSettingRepository = sensorSettingRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<SensorSetting>>> Handle(GetSensorSettingsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<SensorSetting>>(await _sensorSettingRepository.GetListAsync());
            }
        }
    }
}