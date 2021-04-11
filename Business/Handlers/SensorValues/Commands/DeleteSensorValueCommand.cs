
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


namespace Business.Handlers.SensorValues.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteSensorValueCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteSensorValueCommandHandler : IRequestHandler<DeleteSensorValueCommand, IResult>
        {
            private readonly ISensorValueRepository _sensorValueRepository;
            private readonly IMediator _mediator;

            public DeleteSensorValueCommandHandler(ISensorValueRepository sensorValueRepository, IMediator mediator)
            {
                _sensorValueRepository = sensorValueRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteSensorValueCommand request, CancellationToken cancellationToken)
            {
                var sensorValueToDelete = _sensorValueRepository.Get(p => p.Id == request.Id);

                _sensorValueRepository.Delete(sensorValueToDelete);
                await _sensorValueRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

