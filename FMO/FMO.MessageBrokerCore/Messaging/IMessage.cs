using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public interface IMessage
    {
            Guid Id { get; }

            DateTime CreatedDate { get; }

            long Priority { get; set; }

            object Content { get; }

            string QueueName { get; }

            string QueueRootpath { get; }
    }
    //interface IMessage<T> : IMessage
    //{
    //    T GetBody();
    //}

}
