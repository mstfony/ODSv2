
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Settings.ValidationRules;


namespace Business.Handlers.Settings.Commands
{


    public class UpdateSettingCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public double Value { get; set; }

        public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, IResult>
        {
            private readonly ISettingRepository _settingRepository;
            private readonly IMediator _mediator;

            public UpdateSettingCommandHandler(ISettingRepository settingRepository, IMediator mediator)
            {
                _settingRepository = settingRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateSettingValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
            {
                var isThereSettingRecord = await _settingRepository.GetAsync(u => u.Id == request.Id);


                isThereSettingRecord.ParameterId = request.ParameterId;
                isThereSettingRecord.Value = request.Value;


                _settingRepository.Update(isThereSettingRecord);
                await _settingRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

