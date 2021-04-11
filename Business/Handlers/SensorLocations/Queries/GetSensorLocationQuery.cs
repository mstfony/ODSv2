
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.SensorLocations.Queries
{
    public class GetSensorLocationQuery : IRequest<IDataResult<SensorLocation>>
    {
        public int Id { get; set; }

        public class GetSensorLocationQueryHandler : IRequestHandler<GetSensorLocationQuery, IDataResult<SensorLocation>>
        {
            private readonly ISensorLocationRepository _sensorLocationRepository;
            private readonly IMediator _mediator;

            public GetSensorLocationQueryHandler(ISensorLocationRepository sensorLocationRepository, IMediator mediator)
            {
                _sensorLocationRepository = sensorLocationRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<SensorLocation>> Handle(GetSensorLocationQuery request, CancellationToken cancellationToken)
            {
                var sensorLocation = await _sensorLocationRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<SensorLocation>(sensorLocation);
            }
        }
    }
}
