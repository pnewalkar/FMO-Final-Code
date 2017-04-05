using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DataAccessException : Exception
    {
        public DataAccessException(string message)
            : base(message)
        {
        }

        public DataAccessException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DataAccessException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected DataAccessException()
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
