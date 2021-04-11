
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

namespace Business.Handlers.Settings.Queries
{

    public class GetSettingsQuery : IRequest<IDataResult<IEnumerable<Setting>>>
    {
        public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, IDataResult<IEnumerable<Setting>>>
        {
            private readonly ISettingRepository _settingRepository;
            private readonly IMediator _mediator;

            public GetSettingsQueryHandler(ISettingRepository settingRepository, IMediator mediator)
            {
                _settingRepository = settingRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Setting>>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Setting>>(await _settingRepository.GetListAsync());
            }
        }
    }
}