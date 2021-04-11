
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Settings.Queries
{
    public class GetSettingQuery : IRequest<IDataResult<Setting>>
    {
        public int Id { get; set; }

        public class GetSettingQueryHandler : IRequestHandler<GetSettingQuery, IDataResult<Setting>>
        {
            private readonly ISettingRepository _settingRepository;
            private readonly IMediator _mediator;

            public GetSettingQueryHandler(ISettingRepository settingRepository, IMediator mediator)
            {
                _settingRepository = settingRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Setting>> Handle(GetSettingQuery request, CancellationToken cancellationToken)
            {
                var setting = await _settingRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Setting>(setting);
            }
        }
    }
}
