using System;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    [Serializable]
    public class DbConcurrencyException : Exception
    {
        public DbConcurrencyException(string message)
            : base(message)
        {
        }

        public DbConcurrencyException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public DbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DbConcurrencyException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected DbConcurrencyException()
        {
        }
    }
}