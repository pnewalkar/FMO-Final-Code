using Fmo.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Fmo.Common.EmailManagement
{
    public class EmailHelper : IEmailHelper
    {
        private SmtpClient client;

        public EmailHelper()
        {
            client = new SmtpClient();
            client.Host = "localhost";
        }

        public void SendMessage(MailMessage message)
        {
            client.Send(message);
        }
    }
}
