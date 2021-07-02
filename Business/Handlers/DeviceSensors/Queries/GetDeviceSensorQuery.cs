
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.DeviceSensors.Queries
{
    public class GetDeviceSensorQuery : IRequest<IDataResult<DeviceSensor>>
    {
        public int Id { get; set; }

        public class GetDeviceSensorQueryHandler : IRequestHandler<GetDeviceSensorQuery, IDataResult<DeviceSensor>>
        {
            private readonly IDeviceSensorRepository _deviceSensorRepository;
            private readonly IMediator _mediator;

            public GetDeviceSensorQueryHandler(IDeviceSensorRepository deviceSensorRepository, IMediator mediator)
            {
                _deviceSensorRepository = deviceSensorRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<DeviceSensor>> Handle(GetDeviceSensorQuery request, CancellationToken cancellationToken)
            {
                var deviceSensor = await _deviceSensorRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<DeviceSensor>(deviceSensor);
            }
        }
    }
}
