
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

    public class GetAlertActionUsersByAaIdQuery : IRequest<IDataResult<IEnumerable<AlertActionUser>>>
    {
        public int Id { get; set; }
        public class GetAlertActionUsersByAaIdQueryHandler : IRequestHandler<GetAlertActionUsersByAaIdQuery, IDataResult<IEnumerable<AlertActionUser>>>
        {
            private readonly IAlertActionUserRepository _alertActionUserRepository;
            private readonly IMediator _mediator;

            public GetAlertActionUsersByAaIdQueryHandler(IAlertActionUserRepository alertActionUserRepository, IMediator mediator)
            {
                _alertActionUserRepository = alertActionUserRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
           // [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<AlertActionUser>>> Handle(GetAlertActionUsersByAaIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<AlertActionUser>>(await _alertActionUserRepository.GetAlertActionUserByAlertActionId(request.Id));
            }
        }
    }
}