using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Adapters.AlertSevice
{
    public interface IAlertService
    {
        void SendAlert(string paramter,string message);

    }
}
