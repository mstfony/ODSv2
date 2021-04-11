
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
using Business.Handlers.AlertActionLogs.ValidationRules;


namespace Business.Handlers.AlertActionLogs.Commands
{


    public class UpdateAlertActionLogCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int AlertActionUserId { get; set; }
        public System.DateTime DateTime { get; set; }

        public class UpdateAlertActionLogCommandHandler : IRequestHandler<UpdateAlertActionLogCommand, IResult>
        {
            private readonly IAlertActionLogRepository _alertActionLogRepository;
            private readonly IMediator _mediator;

            public UpdateAlertActionLogCommandHandler(IAlertActionLogRepository alertActionLogRepository, IMediator mediator)
            {
                _alertActionLogRepository = alertActionLogRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAlertActionLogValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAlertActionLogCommand request, CancellationToken cancellationToken)
            {
                var isThereAlertActionLogRecord = await _alertActionLogRepository.GetAsync(u => u.Id == request.Id);


                isThereAlertActionLogRecord.AlertActionUserId = request.AlertActionUserId;
                isThereAlertActionLogRecord.DateTime = request.DateTime;


                _alertActionLogRepository.Update(isThereAlertActionLogRecord);
                await _alertActionLogRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

