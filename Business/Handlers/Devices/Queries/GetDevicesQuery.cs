
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Devices.Queries
{

    public class GetDevicesQuery : IRequest<IDataResult<IEnumerable<Device>>>
    {
        public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, IDataResult<IEnumerable<Device>>>
        {
            private readonly IDeviceRepository _deviceRepository;
            private readonly IMediator _mediator;

            public GetDevicesQueryHandler(IDeviceRepository deviceRepository, IMediator mediator)
            {
                _deviceRepository = deviceRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Device>>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Device>>(await _deviceRepository.GetListAsync());
            }
        }
    }
}