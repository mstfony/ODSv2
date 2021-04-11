
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
using Business.Handlers.Sensors.ValidationRules;

namespace Business.Handlers.Sensors.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSensorCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public string Alias { get; set; }


        public class CreateSensorCommandHandler : IRequestHandler<CreateSensorCommand, IResult>
        {
            private readonly ISensorRepository _sensorRepository;
            private readonly IMediator _mediator;
            public CreateSensorCommandHandler(ISensorRepository sensorRepository, IMediator mediator)
            {
                _sensorRepository = sensorRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateSensorValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateSensorCommand request, CancellationToken cancellationToken)
            {
                var isThereSensorRecord = _sensorRepository.Query().Any(u => u.Name == request.Name);

                if (isThereSensorRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedSensor = new Sensor
                {
                    Name = request.Name,
                    Alias = request.Alias,

                };

                _sensorRepository.Add(addedSensor);
                await _sensorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}