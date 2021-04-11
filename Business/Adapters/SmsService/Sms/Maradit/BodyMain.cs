namespace Business.Adapters.SmsService.Sms.Maradit
{
    public class BodyMain
    {
        public BodyMain()
        {
            Credential credential=new Credential();
            Header header=new Header();
        }
        public Credential Credential { get; set; }
        public Header Header { get; set; }
        public string Message { get; set; }
        public string[] To { get; set; }
        public string DataCoding { get; set; }

    }
}
