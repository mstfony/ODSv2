
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
using Business.Handlers.AlertActions.ValidationRules;


namespace Business.Handlers.AlertActions.Commands
{


    public class UpdateAlertActionCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int SensorSettingId { get; set; }
        public int AlertId { get; set; }
        public string Message { get; set; }

        public class UpdateAlertActionCommandHandler : IRequestHandler<UpdateAlertActionCommand, IResult>
        {
            private readonly IAlertActionRepository _alertActionRepository;
            private readonly IMediator _mediator;

            public UpdateAlertActionCommandHandler(IAlertActionRepository alertActionRepository, IMediator mediator)
            {
                _alertActionRepository = alertActionRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAlertActionValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAlertActionCommand request, CancellationToken cancellationToken)
            {
                var isThereAlertActionRecord = await _alertActionRepository.GetAsync(u => u.Id == request.Id);


                isThereAlertActionRecord.SensorSettingId = request.SensorSettingId;
                isThereAlertActionRecord.AlertId = request.AlertId;
                isThereAlertActionRecord.Message = request.Message;


                _alertActionRepository.Update(isThereAlertActionRecord);
                await _alertActionRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

