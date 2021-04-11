
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.SensorSettings.Queries
{
    public class GetSensorSettingQuery : IRequest<IDataResult<SensorSetting>>
    {
        public int Id { get; set; }

        public class GetSensorSettingQueryHandler : IRequestHandler<GetSensorSettingQuery, IDataResult<SensorSetting>>
        {
            private readonly ISensorSettingRepository _sensorSettingRepository;
            private readonly IMediator _mediator;

            public GetSensorSettingQueryHandler(ISensorSettingRepository sensorSettingRepository, IMediator mediator)
            {
                _sensorSettingRepository = sensorSettingRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<SensorSetting>> Handle(GetSensorSettingQuery request, CancellationToken cancellationToken)
            {
                var sensorSetting = await _sensorSettingRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<SensorSetting>(sensorSetting);
            }
        }
    }
}
