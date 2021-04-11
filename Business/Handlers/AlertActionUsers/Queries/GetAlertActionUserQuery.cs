
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.AlertActionUsers.Queries
{
    public class GetAlertActionUserQuery : IRequest<IDataResult<AlertActionUser>>
    {
        public int Id { get; set; }

        public class GetAlertActionUserQueryHandler : IRequestHandler<GetAlertActionUserQuery, IDataResult<AlertActionUser>>
        {
            private readonly IAlertActionUserRepository _alertActionUserRepository;
            private readonly IMediator _mediator;

            public GetAlertActionUserQueryHandler(IAlertActionUserRepository alertActionUserRepository, IMediator mediator)
            {
                _alertActionUserRepository = alertActionUserRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<AlertActionUser>> Handle(GetAlertActionUserQuery request, CancellationToken cancellationToken)
            {
                var alertActionUser = await _alertActionUserRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<AlertActionUser>(alertActionUser);
            }
        }
    }
}
