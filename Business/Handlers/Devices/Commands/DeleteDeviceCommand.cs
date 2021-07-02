
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


namespace Business.Handlers.Devices.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteDeviceCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, IResult>
        {
            private readonly IDeviceRepository _deviceRepository;
            private readonly IMediator _mediator;

            public DeleteDeviceCommandHandler(IDeviceRepository deviceRepository, IMediator mediator)
            {
                _deviceRepository = deviceRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
            {
                var deviceToDelete = _deviceRepository.Get(p => p.Id == request.Id);

                _deviceRepository.Delete(deviceToDelete);
                await _deviceRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

