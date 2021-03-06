
using System;
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
using Business.Adapters.AlertSevice;
using Business.Handlers.AlertActionLogs.Commands;
using Business.Handlers.AlertActionLogs.Queries;
using Business.Handlers.AlertActions.Queries;
using Business.Handlers.AlertActionUsers.Queries;
using Business.Handlers.Parameters.Queries;
using Business.Handlers.SensorSettings.Queries;
using Business.Handlers.SensorValues.ValidationRules;
using Business.Handlers.Settings.Queries;
using Business.Handlers.Users.Queries;
using Core.Utilities.MessageBrokers.RabbitMq;
using Newtonsoft.Json;

namespace Business.Handlers.SensorValues.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSensorValueCommand : IRequest<IResult>
    {

        public SensorValue [] SensorValues { get; set; }
        
        public class CreateSensorValueCommandHandler : IRequestHandler<CreateSensorValueCommand, IResult>
        {
            private readonly ISensorValueRepository _sensorValueRepository;
            private readonly IMediator _mediator;
            private readonly IMessageBrokerHelper _messageBrokerHelper;
            public CreateSensorValueCommandHandler(ISensorValueRepository sensorValueRepository, IMediator mediator,IMessageBrokerHelper messageBrokerHelper)
            {
                _sensorValueRepository = sensorValueRepository;
                _mediator = mediator;
                _messageBrokerHelper = messageBrokerHelper;
            }

            [ValidationAspect(typeof(CreateSensorValueValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateSensorValueCommand request, CancellationToken cancellationToken)
            {
                //RabbitMQ ya gönderir....
                var message = JsonConvert.SerializeObject(request.SensorValues);
                _messageBrokerHelper.QueueMessage(message);

                foreach (var sensorValue in request.SensorValues)
                {
                    var addedSensorValue = new SensorValue
                    {
                        SensorId = sensorValue.SensorId,
                        Value = sensorValue.Value,
                        DateTime = DateTime.Now

                    };

                    _sensorValueRepository.Add(addedSensorValue);
                    await _sensorValueRepository.SaveChangesAsync();


                    //Sensör Setting den Sensor ID ye göre veri çekiyorum
                    //  var sensorSettings = _sensorSettingRepository.GetList(m => m.SensorId == request.SensorId).ToList();
                    var sensorSettings = _mediator.Send(new GetSensorSettingsBySensorIdQuery { Id = sensorValue.SensorId }).Result.Data.ToList();
                    foreach (var sensorSetting in sensorSettings)
                    {
                        // var setting = _settingRepository.Get(m => m.Id == sensorSetting.Id);
                        // var parameter = _parameterRepository.Get(m => m.Id == setting.ParameterId);

                        var setting = _mediator.Send(new GetSettingQuery { Id = sensorSetting.Id }).Result.Data;
                        var parameter = _mediator.Send(new GetParameterQuery { Id = setting.Id }).Result.Data;

                        switch (parameter.Id)
                        {
                            case 1:
                                if (sensorValue.Value >= setting.Value)
                                {
                                    // var alertActions = _alertActionRepository.GetList(m => m.SensorSettingId == sensorSetting.Id).ToList();

                                    AlertSystem(sensorSetting);

                                }
                                break;
                            case 2:
                                if (sensorValue.Value <= setting.Value)
                                {

                                    AlertSystem(sensorSetting);
                                }
                                break;

                        }


                        
                        
                    }
                }
       
                return new SuccessResult(Messages.Added);
            }

            public void AlertSystem(SensorSetting sensorSetting)
            {
                var alertActions = _mediator
                    .Send(new GetAlertActionsBySsIdQuery { Id = sensorSetting.Id }).Result.Data.ToList();

                foreach (var alertAction in alertActions)
                {
                    var users = _mediator.Send(new GetAlertActionUsersByAaIdQuery
                    { Id = alertAction.Id }).Result.Data.ToList();
                    //    var users = _alertActionUserRepository.GetList(m => m.AlertActionId == alertAction.Id).ToList();
                    foreach (var user in users)
                    {

                        var getAlertAction = _mediator
                            .Send(new GetAlertActionUsersByAaIdAndUserIdQuery
                            { AlertId = alertAction.Id, UserId = user.UserId }).Result.Data;

                        var getAlertActionLogs = _mediator
                            .Send(new GetAlertActionLogsByAaIdQuery { id = getAlertAction.Id })
                            .Result.Data.ToList().LastOrDefault();


                        switch (alertAction.AlertId)
                        {
                            case 1:
                                IAlertService alertService = new AlertSms();
                                if (getAlertActionLogs == null)
                                {
                                    // var getUser = _userRepository.Get(m => m.UserId == user.UserId);
                                    var getUser = _mediator.Send(new GetUserQuery { UserId = user.UserId })
                                        .Result.Data;
                                    var log = _mediator.Send(new CreateAlertActionLogCommand
                                    { AlertActionUserId = user.Id, DateTime = DateTime.Now }).Result;


                                    //var getLog=_mediator.Send(new GetAlertActionLogsQuery())
                                    alertService.SendAlert(getUser.MobilePhones, alertAction.Message);

                                }
                                else if (getAlertActionLogs.DateTime.AddMinutes(5) < DateTime.Now)
                                {
                                    // var getUser = _userRepository.Get(m => m.UserId == user.UserId);
                                    var getUser = _mediator.Send(new GetUserQuery { UserId = user.UserId })
                                        .Result.Data;
                                    var log = _mediator.Send(new CreateAlertActionLogCommand
                                    { AlertActionUserId = user.Id, DateTime = DateTime.Now }).Result;


                                    //var getLog=_mediator.Send(new GetAlertActionLogsQuery())
                                    alertService.SendAlert(getUser.MobilePhones, alertAction.Message);
                                }

                                break;
                            case 2:
                                //sendEmail
                                break;
                            case 3:
                                IAlertService alertServiceCall = new AlertCall();
                                if (getAlertActionLogs == null)
                                {
                                    // var getUser = _userRepository.Get(m => m.UserId == user.UserId);
                                    var getUser = _mediator.Send(new GetUserQuery { UserId = user.UserId })
                                        .Result.Data;
                                    var log = _mediator.Send(new CreateAlertActionLogCommand
                                        { AlertActionUserId = user.Id, DateTime = DateTime.Now }).Result;


                                    //var getLog=_mediator.Send(new GetAlertActionLogsQuery())
                                    alertServiceCall.SendAlert(getUser.MobilePhones, alertAction.Message);

                                }
                                else if (getAlertActionLogs.DateTime.AddMinutes(5) < DateTime.Now)
                                {
                                    // var getUser = _userRepository.Get(m => m.UserId == user.UserId);
                                    var getUser = _mediator.Send(new GetUserQuery { UserId = user.UserId })
                                        .Result.Data;
                                    var log = _mediator.Send(new CreateAlertActionLogCommand
                                        { AlertActionUserId = user.Id, DateTime = DateTime.Now }).Result;


                                    //var getLog=_mediator.Send(new GetAlertActionLogsQuery())
                                    alertServiceCall.SendAlert(getUser.MobilePhones, alertAction.Message);
                                }
                                break;
                        }




                    }


                }
            }
        }
    }
}