﻿using System;

namespace Fmo.MessageBrokerCore.Messaging
{
    /// <summary>
    /// IMesssage interface, contains properties that are used by a queue message.
    /// </summary>
    public interface IMessage
    {
            Guid Id { get; }

            DateTime CreatedDate { get; }

            long Priority { get; set; }

            object Content { get; }

            string QueueName { get; }

            string QueueRootpath { get; }
    }
}
