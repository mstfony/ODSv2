
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
using Business.Handlers.Devices.ValidationRules;

namespace Business.Handlers.Devices.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateDeviceCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public string Location { get; set; }
        public string Describe { get; set; }


        public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, IResult>
        {
            private readonly IDeviceRepository _deviceRepository;
            private readonly IMediator _mediator;
            public CreateDeviceCommandHandler(IDeviceRepository deviceRepository, IMediator mediator)
            {
                _deviceRepository = deviceRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateDeviceValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
            {
                var isThereDeviceRecord = _deviceRepository.Query().Any(u => u.Name == request.Name);

                if (isThereDeviceRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedDevice = new Device
                {
                    Name = request.Name,
                    Location = request.Location,
                    Describe = request.Describe,

                };

                _deviceRepository.Add(addedDevice);
                await _deviceRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}