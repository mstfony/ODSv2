
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
using Business.Handlers.AlertActionLogs.ValidationRules;

namespace Business.Handlers.AlertActionLogs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAlertActionLogCommand : IRequest<IResult>
    {

        public int AlertActionUserId { get; set; }
        public System.DateTime DateTime { get; set; }


        public class CreateAlertActionLogCommandHandler : IRequestHandler<CreateAlertActionLogCommand, IResult>
        {
            private readonly IAlertActionLogRepository _alertActionLogRepository;
            private readonly IMediator _mediator;
            public CreateAlertActionLogCommandHandler(IAlertActionLogRepository alertActionLogRepository, IMediator mediator)
            {
                _alertActionLogRepository = alertActionLogRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAlertActionLogValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAlertActionLogCommand request, CancellationToken cancellationToken)
            {
                //var isThereAlertActionLogRecord = _alertActionLogRepository.Query().Any(u => u.AlertActionUserId == request.AlertActionUserId);

                //if (isThereAlertActionLogRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);

                var addedAlertActionLog = new AlertActionLog
                {
                    AlertActionUserId = request.AlertActionUserId,
                    DateTime = request.DateTime,

                };

                _alertActionLogRepository.Add(addedAlertActionLog);
                await _alertActionLogRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}