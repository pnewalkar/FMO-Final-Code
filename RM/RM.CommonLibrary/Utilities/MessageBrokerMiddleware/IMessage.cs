using System;

namespace RM.CommonLibrary.MessageBrokerMiddleware
{
    /// <summary>
    /// IMesssage interface, contains properties that are used by a queue message. Message object with all the details of the message to be queued.
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