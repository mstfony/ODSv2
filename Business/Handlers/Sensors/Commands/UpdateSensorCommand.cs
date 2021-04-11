
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
using Business.Handlers.Sensors.ValidationRules;


namespace Business.Handlers.Sensors.Commands
{


    public class UpdateSensorCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public class UpdateSensorCommandHandler : IRequestHandler<UpdateSensorCommand, IResult>
        {
            private readonly ISensorRepository _sensorRepository;
            private readonly IMediator _mediator;

            public UpdateSensorCommandHandler(ISensorRepository sensorRepository, IMediator mediator)
            {
                _sensorRepository = sensorRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateSensorValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateSensorCommand request, CancellationToken cancellationToken)
            {
                var isThereSensorRecord = await _sensorRepository.GetAsync(u => u.Id == request.Id);


                isThereSensorRecord.Name = request.Name;
                isThereSensorRecord.Alias = request.Alias;


                _sensorRepository.Update(isThereSensorRecord);
                await _sensorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

