using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Adapters.SmsService.Sms.Maradit;


namespace Business.Adapters.SmsService
{
    public class SmsServiceManager : ISmsService
    {
        public async Task<bool> Send(string password, string text, string cellPhone)
        {
            Thread.Sleep(1000);
            return await Task.FromResult(true);
        }
        public async Task<bool> SendAssist(string cellPhone, string text)
        {
            HttpClient httpClient = new HttpClient();
            string postUrl = "http://gw.maradit.net/api/json/reply/Submit?";

            string[] cepTelList = new[] { cellPhone };

            Credential credential = new Credential();
            credential.Username = "hatem";
            credential.Password = "Bim2020**!";

            Header header = new Header();
            header.From = "HATEMHSTNSI";

            BodyMain body = new BodyMain();
            body.Credential = credential;
            body.Header = header;
            body.Message = text;
            body.To = cepTelList;
            body.DataCoding = "Default";

            string jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            var postResponse = await httpClient.PostAsync(postUrl, content);

            postResponse.EnsureSuccessStatusCode();
            return await Task.FromResult(true);
        }
    }
}
