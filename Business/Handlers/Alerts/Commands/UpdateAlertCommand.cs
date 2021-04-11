
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
using Business.Handlers.Alerts.ValidationRules;


namespace Business.Handlers.Alerts.Commands
{


    public class UpdateAlertCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateAlertCommandHandler : IRequestHandler<UpdateAlertCommand, IResult>
        {
            private readonly IAlertRepository _alertRepository;
            private readonly IMediator _mediator;

            public UpdateAlertCommandHandler(IAlertRepository alertRepository, IMediator mediator)
            {
                _alertRepository = alertRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAlertValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAlertCommand request, CancellationToken cancellationToken)
            {
                var isThereAlertRecord = await _alertRepository.GetAsync(u => u.Id == request.Id);


                isThereAlertRecord.Name = request.Name;


                _alertRepository.Update(isThereAlertRecord);
                await _alertRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

