
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


namespace Business.Handlers.Locations.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteLocationCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, IResult>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public DeleteLocationCommandHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
            {
                var locationToDelete = _locationRepository.Get(p => p.Id == request.Id);

                _locationRepository.Delete(locationToDelete);
                await _locationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

