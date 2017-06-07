using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message)
            : base(message)
        {
        }

        public BusinessLogicException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BusinessLogicException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected BusinessLogicException()
        {
        }

        protected BusinessLogicException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }
    }
}