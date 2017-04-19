using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    /// <summary>
    /// Assists in creation of the Message Adapter instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MessageAdapterFactory<T>
    {
        //provides an adapter for the queueing technology that we are currently using
        internal IMessageAdapter<T> GetAdapter(string queueName, string queueRootPath)
        {
            return new MessageAdapter<T>(queueName, queueRootPath);
        }
    }
}
