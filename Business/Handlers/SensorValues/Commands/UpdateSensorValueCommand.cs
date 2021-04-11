
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
using Business.Handlers.SensorValues.ValidationRules;


namespace Business.Handlers.SensorValues.Commands
{


    public class UpdateSensorValueCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public double Value { get; set; }
        public System.DateTime DateTime { get; set; }

        public class UpdateSensorValueCommandHandler : IRequestHandler<UpdateSensorValueCommand, IResult>
        {
            private readonly ISensorValueRepository _sensorValueRepository;
            private readonly IMediator _mediator;

            public UpdateSensorValueCommandHandler(ISensorValueRepository sensorValueRepository, IMediator mediator)
            {
                _sensorValueRepository = sensorValueRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateSensorValueValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateSensorValueCommand request, CancellationToken cancellationToken)
            {
                var isThereSensorValueRecord = await _sensorValueRepository.GetAsync(u => u.Id == request.Id);


                isThereSensorValueRecord.SensorId = request.SensorId;
                isThereSensorValueRecord.Value = request.Value;
                isThereSensorValueRecord.DateTime = request.DateTime;


                _sensorValueRepository.Update(isThereSensorValueRecord);
                await _sensorValueRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

