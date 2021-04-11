
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.SensorValues.Queries
{
    public class GetSensorValueQuery : IRequest<IDataResult<SensorValue>>
    {
        public int Id { get; set; }

        public class GetSensorValueQueryHandler : IRequestHandler<GetSensorValueQuery, IDataResult<SensorValue>>
        {
            private readonly ISensorValueRepository _sensorValueRepository;
            private readonly IMediator _mediator;

            public GetSensorValueQueryHandler(ISensorValueRepository sensorValueRepository, IMediator mediator)
            {
                _sensorValueRepository = sensorValueRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<SensorValue>> Handle(GetSensorValueQuery request, CancellationToken cancellationToken)
            {
                var sensorValue = await _sensorValueRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<SensorValue>(sensorValue);
            }
        }
    }
}
