
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


namespace Business.Handlers.SensorLocations.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteSensorLocationCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteSensorLocationCommandHandler : IRequestHandler<DeleteSensorLocationCommand, IResult>
        {
            private readonly ISensorLocationRepository _sensorLocationRepository;
            private readonly IMediator _mediator;

            public DeleteSensorLocationCommandHandler(ISensorLocationRepository sensorLocationRepository, IMediator mediator)
            {
                _sensorLocationRepository = sensorLocationRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteSensorLocationCommand request, CancellationToken cancellationToken)
            {
                var sensorLocationToDelete = _sensorLocationRepository.Get(p => p.Id == request.Id);

                _sensorLocationRepository.Delete(sensorLocationToDelete);
                await _sensorLocationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

