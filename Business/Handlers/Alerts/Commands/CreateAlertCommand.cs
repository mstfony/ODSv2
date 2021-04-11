
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
using Business.Handlers.Alerts.ValidationRules;

namespace Business.Handlers.Alerts.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAlertCommand : IRequest<IResult>
    {

        public string Name { get; set; }


        public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, IResult>
        {
            private readonly IAlertRepository _alertRepository;
            private readonly IMediator _mediator;
            public CreateAlertCommandHandler(IAlertRepository alertRepository, IMediator mediator)
            {
                _alertRepository = alertRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAlertValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
            {
                var isThereAlertRecord = _alertRepository.Query().Any(u => u.Name == request.Name);

                if (isThereAlertRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedAlert = new Alert
                {
                    Name = request.Name,

                };

                _alertRepository.Add(addedAlert);
                await _alertRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}