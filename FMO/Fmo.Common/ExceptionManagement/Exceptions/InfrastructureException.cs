using System;
using System.Diagnostics.CodeAnalysis;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InfrastructureException : Exception
    {
        protected InfrastructureException()
        {

        }
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
    }
}
