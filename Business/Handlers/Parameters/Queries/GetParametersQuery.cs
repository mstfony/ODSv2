
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

namespace Business.Handlers.Parameters.Queries
{

    public class GetParametersQuery : IRequest<IDataResult<IEnumerable<Parameter>>>
    {
        public class GetParametersQueryHandler : IRequestHandler<GetParametersQuery, IDataResult<IEnumerable<Parameter>>>
        {
            private readonly IParameterRepository _parameterRepository;
            private readonly IMediator _mediator;

            public GetParametersQueryHandler(IParameterRepository parameterRepository, IMediator mediator)
            {
                _parameterRepository = parameterRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Parameter>>> Handle(GetParametersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Parameter>>(await _parameterRepository.GetListAsync());
            }
        }
    }
}