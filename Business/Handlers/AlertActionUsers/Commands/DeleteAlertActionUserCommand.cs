
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


namespace Business.Handlers.AlertActionUsers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAlertActionUserCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteAlertActionUserCommandHandler : IRequestHandler<DeleteAlertActionUserCommand, IResult>
        {
            private readonly IAlertActionUserRepository _alertActionUserRepository;
            private readonly IMediator _mediator;

            public DeleteAlertActionUserCommandHandler(IAlertActionUserRepository alertActionUserRepository, IMediator mediator)
            {
                _alertActionUserRepository = alertActionUserRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAlertActionUserCommand request, CancellationToken cancellationToken)
            {
                var alertActionUserToDelete = _alertActionUserRepository.Get(p => p.Id == request.Id);

                _alertActionUserRepository.Delete(alertActionUserToDelete);
                await _alertActionUserRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

