
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
using Business.Handlers.AlertActionUsers.ValidationRules;

namespace Business.Handlers.AlertActionUsers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAlertActionUserCommand : IRequest<IResult>
    {

        public int AlertActionId { get; set; }
        public int UserId { get; set; }


        public class CreateAlertActionUserCommandHandler : IRequestHandler<CreateAlertActionUserCommand, IResult>
        {
            private readonly IAlertActionUserRepository _alertActionUserRepository;
            private readonly IMediator _mediator;
            public CreateAlertActionUserCommandHandler(IAlertActionUserRepository alertActionUserRepository, IMediator mediator)
            {
                _alertActionUserRepository = alertActionUserRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAlertActionUserValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAlertActionUserCommand request, CancellationToken cancellationToken)
            {
                var isThereAlertActionUserRecord = _alertActionUserRepository.Query().Any(u => u.AlertActionId == request.AlertActionId);

                if (isThereAlertActionUserRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedAlertActionUser = new AlertActionUser
                {
                    AlertActionId = request.AlertActionId,
                    UserId = request.UserId,

                };

                _alertActionUserRepository.Add(addedAlertActionUser);
                await _alertActionUserRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}