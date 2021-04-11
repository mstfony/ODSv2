
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Parameters.Queries
{
    public class GetParameterQuery : IRequest<IDataResult<Parameter>>
    {
        public int Id { get; set; }

        public class GetParameterQueryHandler : IRequestHandler<GetParameterQuery, IDataResult<Parameter>>
        {
            private readonly IParameterRepository _parameterRepository;
            private readonly IMediator _mediator;

            public GetParameterQueryHandler(IParameterRepository parameterRepository, IMediator mediator)
            {
                _parameterRepository = parameterRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Parameter>> Handle(GetParameterQuery request, CancellationToken cancellationToken)
            {
                var parameter = await _parameterRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Parameter>(parameter);
            }
        }
    }
}
