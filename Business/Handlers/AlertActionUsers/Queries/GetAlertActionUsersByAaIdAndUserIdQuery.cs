
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.AlertActionUsers.Queries
{

    public class GetAlertActionUsersByAaIdAndUserIdQuery : IRequest<IDataResult<AlertActionUser>>
    {
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public class GetAlertActionUsersByAaIdAndUserIdQueryHandler : IRequestHandler<GetAlertActionUsersByAaIdAndUserIdQuery, IDataResult<AlertActionUser>>
        {
            private readonly IAlertActionUserRepository _alertActionUserRepository;
            private readonly IMediator _mediator;

            public GetAlertActionUsersByAaIdAndUserIdQueryHandler(IAlertActionUserRepository alertActionUserRepository, IMediator mediator)
            {
                _alertActionUserRepository = alertActionUserRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
           // [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<AlertActionUser>> Handle(GetAlertActionUsersByAaIdAndUserIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<AlertActionUser>(await _alertActionUserRepository.GetAlertActionUserByAlertActionIdAndUserId(request.AlertId,request.UserId));
            }
        }
    }
}