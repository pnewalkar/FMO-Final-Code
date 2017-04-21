namespace Fmo.Common.EmailManagement
{
    using System.Net.Mail;
    using Fmo.Common.Constants;
    using Fmo.Common.Interface;

    public class EmailHelper : IEmailHelper
    {
        private SmtpClient client;

        public void SendMessage(MailMessage message)
        {
            using (client = new SmtpClient())
            {
                client.Host = Constants.SMTPHOSTNAME;
                client.Port = Constants.SMTPPORT;
                client.Send(message);
            }
        }
    }
}
