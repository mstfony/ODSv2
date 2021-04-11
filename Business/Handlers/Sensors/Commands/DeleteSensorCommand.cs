
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


namespace Business.Handlers.Sensors.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteSensorCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteSensorCommandHandler : IRequestHandler<DeleteSensorCommand, IResult>
        {
            private readonly ISensorRepository _sensorRepository;
            private readonly IMediator _mediator;

            public DeleteSensorCommandHandler(ISensorRepository sensorRepository, IMediator mediator)
            {
                _sensorRepository = sensorRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteSensorCommand request, CancellationToken cancellationToken)
            {
                var sensorToDelete = _sensorRepository.Get(p => p.Id == request.Id);

                _sensorRepository.Delete(sensorToDelete);
                await _sensorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

