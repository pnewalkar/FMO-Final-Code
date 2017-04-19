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
        /// <summary>
        /// Provides an adapter for the queueing technology that we are currently using
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns></returns>
        internal IMessageAdapter<T> GetAdapter(string queueName, string queueRootPath)
        {
            return new MessageAdapter<T>(queueName, queueRootPath);
        }
    }
}
