using System;
using System.Diagnostics.CodeAnalysis;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MailSendException : Exception
    {
        public MailSendException(string message)
            : base(message)
        {
        }

        public MailSendException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public MailSendException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MailSendException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected MailSendException()
        {
        }
    }
}
