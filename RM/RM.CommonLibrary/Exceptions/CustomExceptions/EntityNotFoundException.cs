﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    [SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Justification = "Not applicable for this class.")]
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

        public EntityNotFoundException(Type entityType, string message, params object[] args)
        : base(message, args)
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Type EntityType { get; private set; }
    }
}