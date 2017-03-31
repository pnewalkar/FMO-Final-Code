using System;
using System.Diagnostics.CodeAnalysis;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class EntityAlreadyExistsException : DataAccessException
    { 
        public EntityAlreadyExistsException(string message, params object[] args)
            : base(message, args)
        {
        }
    }
}
