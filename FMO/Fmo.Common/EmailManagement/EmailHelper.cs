namespace Fmo.Common.EmailManagement
{
    using System.Net.Mail;
    using Fmo.Common.Interface;

    public class EmailHelper : IEmailHelper
    {
        private SmtpClient client;

        public EmailHelper()
        {
            client = new SmtpClient();
            client.Host = string.Empty;
        }

        public void SendMessage(MailMessage message)
        {
            client.Send(message);
        }
    }
}
