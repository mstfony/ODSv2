
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.AlertActions.Queries
{
    public class GetAlertActionQuery : IRequest<IDataResult<AlertAction>>
    {
        public int Id { get; set; }

        public class GetAlertActionQueryHandler : IRequestHandler<GetAlertActionQuery, IDataResult<AlertAction>>
        {
            private readonly IAlertActionRepository _alertActionRepository;
            private readonly IMediator _mediator;

            public GetAlertActionQueryHandler(IAlertActionRepository alertActionRepository, IMediator mediator)
            {
                _alertActionRepository = alertActionRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<AlertAction>> Handle(GetAlertActionQuery request, CancellationToken cancellationToken)
            {
                var alertAction = await _alertActionRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<AlertAction>(alertAction);
            }
        }
    }
}
