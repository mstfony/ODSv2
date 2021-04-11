
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
using Business.Handlers.AlertActionUsers.ValidationRules;


namespace Business.Handlers.AlertActionUsers.Commands
{


    public class UpdateAlertActionUserCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int AlertActionId { get; set; }
        public int UserId { get; set; }

        public class UpdateAlertActionUserCommandHandler : IRequestHandler<UpdateAlertActionUserCommand, IResult>
        {
            private readonly IAlertActionUserRepository _alertActionUserRepository;
            private readonly IMediator _mediator;

            public UpdateAlertActionUserCommandHandler(IAlertActionUserRepository alertActionUserRepository, IMediator mediator)
            {
                _alertActionUserRepository = alertActionUserRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAlertActionUserValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAlertActionUserCommand request, CancellationToken cancellationToken)
            {
                var isThereAlertActionUserRecord = await _alertActionUserRepository.GetAsync(u => u.Id == request.Id);


                isThereAlertActionUserRecord.AlertActionId = request.AlertActionId;
                isThereAlertActionUserRecord.UserId = request.UserId;


                _alertActionUserRepository.Update(isThereAlertActionUserRecord);
                await _alertActionUserRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

