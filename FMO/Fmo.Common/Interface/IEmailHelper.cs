using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Interface
{
    public interface IEmailHelper
    {
        void SendMessage(MailMessage message);
    }
}
