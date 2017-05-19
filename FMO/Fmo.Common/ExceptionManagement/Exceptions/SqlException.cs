using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class SqlException : Exception
    {
        public SqlException(string message)
            : base(message)
        {
        }

        public SqlException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public SqlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SqlException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected SqlException()
        {
        }

        protected SqlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}