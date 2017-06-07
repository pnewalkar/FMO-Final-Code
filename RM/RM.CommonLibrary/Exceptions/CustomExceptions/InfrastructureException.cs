using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message)
            : base(message)
        {
        }

        public InfrastructureException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public InfrastructureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InfrastructureException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected InfrastructureException()
        {
        }

        protected InfrastructureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}