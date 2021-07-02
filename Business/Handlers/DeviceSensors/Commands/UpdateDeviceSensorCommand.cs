
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.DeviceSensors.ValidationRules;


namespace Business.Handlers.DeviceSensors.Commands
{


    public class UpdateDeviceSensorCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int SensorId { get; set; }

        public class UpdateDeviceSensorCommandHandler : IRequestHandler<UpdateDeviceSensorCommand, IResult>
        {
            private readonly IDeviceSensorRepository _deviceSensorRepository;
            private readonly IMediator _mediator;

            public UpdateDeviceSensorCommandHandler(IDeviceSensorRepository deviceSensorRepository, IMediator mediator)
            {
                _deviceSensorRepository = deviceSensorRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateDeviceSensorValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateDeviceSensorCommand request, CancellationToken cancellationToken)
            {
                var isThereDeviceSensorRecord = await _deviceSensorRepository.GetAsync(u => u.Id == request.Id);


                isThereDeviceSensorRecord.DeviceId = request.DeviceId;
                isThereDeviceSensorRecord.SensorId = request.SensorId;


                _deviceSensorRepository.Update(isThereDeviceSensorRecord);
                await _deviceSensorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

