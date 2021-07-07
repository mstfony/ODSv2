using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Business.Adapters.AlertSevice
{
    public class AlertCall : IAlertService
    {
        public void SendAlert(string paramter, string message)
        {
            String token = "hgovFjT29x";
            String data1 = "{\"token\": \"" + token + "\" ,\"do\":\"" + message + "\",\"numbers\":[\"" + paramter + "\"] }";
            using (WebClient client = new WebClient())
            {
                try
                {
                    #region POST YAPILIYOR
                    string postUrl = "http://192.168.1.19/sensor/api.php";
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    string result = client.UploadString(postUrl, "POST", data1);
                    #endregion
                    #region POST NETİCESİNDE ÇIKTI ALINIYOR

                    #endregion
                }
                catch
                {

                }
            }
        }
    }
}
