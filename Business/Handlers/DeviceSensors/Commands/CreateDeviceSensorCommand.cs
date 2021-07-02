
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.DeviceSensors.ValidationRules;

namespace Business.Handlers.DeviceSensors.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateDeviceSensorCommand : IRequest<IResult>
    {

        public int DeviceId { get; set; }
        public int SensorId { get; set; }


        public class CreateDeviceSensorCommandHandler : IRequestHandler<CreateDeviceSensorCommand, IResult>
        {
            private readonly IDeviceSensorRepository _deviceSensorRepository;
            private readonly IMediator _mediator;
            public CreateDeviceSensorCommandHandler(IDeviceSensorRepository deviceSensorRepository, IMediator mediator)
            {
                _deviceSensorRepository = deviceSensorRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateDeviceSensorValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateDeviceSensorCommand request, CancellationToken cancellationToken)
            {
               

                var addedDeviceSensor = new DeviceSensor
                {
                    DeviceId = request.DeviceId,
                    SensorId = request.SensorId,

                };

                _deviceSensorRepository.Add(addedDeviceSensor);
                await _deviceSensorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}