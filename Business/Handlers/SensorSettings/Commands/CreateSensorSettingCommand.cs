
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
using Business.Handlers.SensorSettings.ValidationRules;

namespace Business.Handlers.SensorSettings.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSensorSettingCommand : IRequest<IResult>
    {

        public int SensorId { get; set; }
        public int SettingId { get; set; }


        public class CreateSensorSettingCommandHandler : IRequestHandler<CreateSensorSettingCommand, IResult>
        {
            private readonly ISensorSettingRepository _sensorSettingRepository;
            private readonly IMediator _mediator;
            public CreateSensorSettingCommandHandler(ISensorSettingRepository sensorSettingRepository, IMediator mediator)
            {
                _sensorSettingRepository = sensorSettingRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateSensorSettingValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateSensorSettingCommand request, CancellationToken cancellationToken)
            {
              

                var addedSensorSetting = new SensorSetting
                {
                    SensorId = request.SensorId,
                    SettingId = request.SettingId,

                };

                _sensorSettingRepository.Add(addedSensorSetting);
                await _sensorSettingRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}