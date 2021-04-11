
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


namespace Business.Handlers.Settings.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteSettingCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteSettingCommandHandler : IRequestHandler<DeleteSettingCommand, IResult>
        {
            private readonly ISettingRepository _settingRepository;
            private readonly IMediator _mediator;

            public DeleteSettingCommandHandler(ISettingRepository settingRepository, IMediator mediator)
            {
                _settingRepository = settingRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
            {
                var settingToDelete = _settingRepository.Get(p => p.Id == request.Id);

                _settingRepository.Delete(settingToDelete);
                await _settingRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

