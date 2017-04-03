using System;
using System.Diagnostics.CodeAnalysis;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ServiceException : Exception
    {
        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ServiceException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected ServiceException()
        {
        }
    }
}
