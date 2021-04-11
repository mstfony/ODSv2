using System;
using System.Collections.Generic;
using System.Text;
using Business.Adapters.SmsService;
using Business.Handlers.Users.Queries;
using MediatR;

namespace Business.Adapters.AlertSevice
{
    public class AlertSms : IAlertService
    {
        public void SendAlert(string paramter, string message)
        {
            ISmsService smsService=new SmsServiceManager();
            smsService.SendAssist(paramter, message);
        }
    }
}
