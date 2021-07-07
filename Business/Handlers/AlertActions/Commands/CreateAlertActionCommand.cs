
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
using Business.Handlers.AlertActions.ValidationRules;

namespace Business.Handlers.AlertActions.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAlertActionCommand : IRequest<IResult>
    {

        public int SensorSettingId { get; set; }
        public int AlertId { get; set; }
        public string Message { get; set; }


        public class CreateAlertActionCommandHandler : IRequestHandler<CreateAlertActionCommand, IResult>
        {
            private readonly IAlertActionRepository _alertActionRepository;
            private readonly IMediator _mediator;
            public CreateAlertActionCommandHandler(IAlertActionRepository alertActionRepository, IMediator mediator)
            {
                _alertActionRepository = alertActionRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAlertActionValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAlertActionCommand request, CancellationToken cancellationToken)
            {
                //var isThereAlertActionRecord = _alertActionRepository.Query().Any(u => u.SensorSettingId == request.SensorSettingId);

                //if (isThereAlertActionRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);

                var addedAlertAction = new AlertAction
                {
                    SensorSettingId = request.SensorSettingId,
                    AlertId = request.AlertId,
                    Message = request.Message

                };

                _alertActionRepository.Add(addedAlertAction);
                await _alertActionRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}