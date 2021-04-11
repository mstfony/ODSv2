
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Sensors.Queries
{
    public class GetSensorQuery : IRequest<IDataResult<Sensor>>
    {
        public int Id { get; set; }

        public class GetSensorQueryHandler : IRequestHandler<GetSensorQuery, IDataResult<Sensor>>
        {
            private readonly ISensorRepository _sensorRepository;
            private readonly IMediator _mediator;

            public GetSensorQueryHandler(ISensorRepository sensorRepository, IMediator mediator)
            {
                _sensorRepository = sensorRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Sensor>> Handle(GetSensorQuery request, CancellationToken cancellationToken)
            {
                var sensor = await _sensorRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Sensor>(sensor);
            }
        }
    }
}
