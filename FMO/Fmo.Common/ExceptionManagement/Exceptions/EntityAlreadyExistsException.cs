using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

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

        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
