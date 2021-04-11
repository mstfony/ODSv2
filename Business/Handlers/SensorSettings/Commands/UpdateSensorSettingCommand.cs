
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
using Business.Handlers.SensorSettings.ValidationRules;


namespace Business.Handlers.SensorSettings.Commands
{


    public class UpdateSensorSettingCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public int SettingId { get; set; }

        public class UpdateSensorSettingCommandHandler : IRequestHandler<UpdateSensorSettingCommand, IResult>
        {
            private readonly ISensorSettingRepository _sensorSettingRepository;
            private readonly IMediator _mediator;

            public UpdateSensorSettingCommandHandler(ISensorSettingRepository sensorSettingRepository, IMediator mediator)
            {
                _sensorSettingRepository = sensorSettingRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateSensorSettingValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateSensorSettingCommand request, CancellationToken cancellationToken)
            {
                var isThereSensorSettingRecord = await _sensorSettingRepository.GetAsync(u => u.Id == request.Id);


                isThereSensorSettingRecord.SensorId = request.SensorId;
                isThereSensorSettingRecord.SettingId = request.SettingId;


                _sensorSettingRepository.Update(isThereSensorSettingRecord);
                await _sensorSettingRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

