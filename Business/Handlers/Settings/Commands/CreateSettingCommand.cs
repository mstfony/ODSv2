
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Settings.ValidationRules;

namespace Business.Handlers.Settings.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSettingCommand : IRequest<IResult>
    {

        public int ParameterId { get; set; }
        public double Value { get; set; }


        public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, IResult>
        {
            private readonly ISettingRepository _settingRepository;
            private readonly IMediator _mediator;
            public CreateSettingCommandHandler(ISettingRepository settingRepository, IMediator mediator)
            {
                _settingRepository = settingRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateSettingValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
            {
              

                var addedSetting = new Setting
                {
                    ParameterId = request.ParameterId,
                    Value = request.Value,

                };

                _settingRepository.Add(addedSetting);
                await _settingRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}