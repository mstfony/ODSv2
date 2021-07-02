
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Devices.Queries
{
    public class GetDeviceQuery : IRequest<IDataResult<Device>>
    {
        public int Id { get; set; }

        public class GetDeviceQueryHandler : IRequestHandler<GetDeviceQuery, IDataResult<Device>>
        {
            private readonly IDeviceRepository _deviceRepository;
            private readonly IMediator _mediator;

            public GetDeviceQueryHandler(IDeviceRepository deviceRepository, IMediator mediator)
            {
                _deviceRepository = deviceRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Device>> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
            {
                var device = await _deviceRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Device>(device);
            }
        }
    }
}
