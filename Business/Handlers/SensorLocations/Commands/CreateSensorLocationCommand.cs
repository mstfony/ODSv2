
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
using Business.Handlers.SensorLocations.ValidationRules;

namespace Business.Handlers.SensorLocations.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSensorLocationCommand : IRequest<IResult>
    {

        public int SensorId { get; set; }
        public int LocationId { get; set; }


        public class CreateSensorLocationCommandHandler : IRequestHandler<CreateSensorLocationCommand, IResult>
        {
            private readonly ISensorLocationRepository _sensorLocationRepository;
            private readonly IMediator _mediator;
            public CreateSensorLocationCommandHandler(ISensorLocationRepository sensorLocationRepository, IMediator mediator)
            {
                _sensorLocationRepository = sensorLocationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateSensorLocationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateSensorLocationCommand request, CancellationToken cancellationToken)
            {
                var isThereSensorLocationRecord = _sensorLocationRepository.Query().Any(u => u.SensorId == request.SensorId);

                if (isThereSensorLocationRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedSensorLocation = new SensorLocation
                {
                    SensorId = request.SensorId,
                    LocationId = request.LocationId,

                };

                _sensorLocationRepository.Add(addedSensorLocation);
                await _sensorLocationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}