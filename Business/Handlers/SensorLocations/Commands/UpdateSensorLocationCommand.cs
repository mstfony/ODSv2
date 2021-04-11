
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
using Business.Handlers.SensorLocations.ValidationRules;


namespace Business.Handlers.SensorLocations.Commands
{


    public class UpdateSensorLocationCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public int LocationId { get; set; }

        public class UpdateSensorLocationCommandHandler : IRequestHandler<UpdateSensorLocationCommand, IResult>
        {
            private readonly ISensorLocationRepository _sensorLocationRepository;
            private readonly IMediator _mediator;

            public UpdateSensorLocationCommandHandler(ISensorLocationRepository sensorLocationRepository, IMediator mediator)
            {
                _sensorLocationRepository = sensorLocationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateSensorLocationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateSensorLocationCommand request, CancellationToken cancellationToken)
            {
                var isThereSensorLocationRecord = await _sensorLocationRepository.GetAsync(u => u.Id == request.Id);


                isThereSensorLocationRecord.SensorId = request.SensorId;
                isThereSensorLocationRecord.LocationId = request.LocationId;


                _sensorLocationRepository.Update(isThereSensorLocationRecord);
                await _sensorLocationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

