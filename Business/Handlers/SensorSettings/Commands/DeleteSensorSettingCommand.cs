
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


namespace Business.Handlers.SensorSettings.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteSensorSettingCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteSensorSettingCommandHandler : IRequestHandler<DeleteSensorSettingCommand, IResult>
        {
            private readonly ISensorSettingRepository _sensorSettingRepository;
            private readonly IMediator _mediator;

            public DeleteSensorSettingCommandHandler(ISensorSettingRepository sensorSettingRepository, IMediator mediator)
            {
                _sensorSettingRepository = sensorSettingRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteSensorSettingCommand request, CancellationToken cancellationToken)
            {
                var sensorSettingToDelete = _sensorSettingRepository.Get(p => p.Id == request.Id);

                _sensorSettingRepository.Delete(sensorSettingToDelete);
                await _sensorSettingRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

