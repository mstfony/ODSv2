
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.DeviceSensors.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteDeviceSensorCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteDeviceSensorCommandHandler : IRequestHandler<DeleteDeviceSensorCommand, IResult>
        {
            private readonly IDeviceSensorRepository _deviceSensorRepository;
            private readonly IMediator _mediator;

            public DeleteDeviceSensorCommandHandler(IDeviceSensorRepository deviceSensorRepository, IMediator mediator)
            {
                _deviceSensorRepository = deviceSensorRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteDeviceSensorCommand request, CancellationToken cancellationToken)
            {
                var deviceSensorToDelete = _deviceSensorRepository.Get(p => p.Id == request.Id);

                _deviceSensorRepository.Delete(deviceSensorToDelete);
                await _deviceSensorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

