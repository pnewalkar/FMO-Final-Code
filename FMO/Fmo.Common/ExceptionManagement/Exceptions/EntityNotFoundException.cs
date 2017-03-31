using System;
using System.Diagnostics.CodeAnalysis;

namespace Fmo.Common.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class EntityNotFoundException : DataAccessException
    {
        public EntityNotFoundException(Type entityType)
            : base("Cannot find entity of type {0}.", entityType.Name)
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(string message, params object[] args)
            : base(message, args)
        {
        }

        public EntityNotFoundException(Type entityType, string message, params object[] args) : base(message, args)
        {
            EntityType = entityType;
        }

        public Type EntityType { get; private set; }
    }
}
