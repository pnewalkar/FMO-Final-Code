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
        SmtpClient _client;

        public EmailHelper()
        {
            _client = new SmtpClient();
            _client.Host = "localhost";
        }

        public void SendMessage(MailMessage message)
        {
            _client.Send(message);
        }
    }
}
