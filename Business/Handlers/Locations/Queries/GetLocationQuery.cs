
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Locations.Queries
{
    public class GetLocationQuery : IRequest<IDataResult<Location>>
    {
        public int Id { get; set; }

        public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, IDataResult<Location>>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public GetLocationQueryHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Location>> Handle(GetLocationQuery request, CancellationToken cancellationToken)
            {
                var location = await _locationRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Location>(location);
            }
        }
    }
}
