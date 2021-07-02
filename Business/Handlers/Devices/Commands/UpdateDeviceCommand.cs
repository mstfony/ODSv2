
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
using Business.Handlers.Devices.ValidationRules;


namespace Business.Handlers.Devices.Commands
{


    public class UpdateDeviceCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Describe { get; set; }

        public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, IResult>
        {
            private readonly IDeviceRepository _deviceRepository;
            private readonly IMediator _mediator;

            public UpdateDeviceCommandHandler(IDeviceRepository deviceRepository, IMediator mediator)
            {
                _deviceRepository = deviceRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateDeviceValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
            {
                var isThereDeviceRecord = await _deviceRepository.GetAsync(u => u.Id == request.Id);


                isThereDeviceRecord.Name = request.Name;
                isThereDeviceRecord.Location = request.Location;
                isThereDeviceRecord.Describe = request.Describe;


                _deviceRepository.Update(isThereDeviceRecord);
                await _deviceRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

