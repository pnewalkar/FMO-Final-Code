using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class WebServiceException : Exception
    {
        public WebServiceException(string message)
            : base(message)
        {
        }

        public WebServiceException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public WebServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public WebServiceException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected WebServiceException()
        {
        }

        protected WebServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}