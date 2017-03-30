using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
                //    //MessageQueue messageQueue = null;
    internal class MessageAdapterFactory<T>
    {
        //provides an adapter for the queueing technology that we are currently using
        internal IMessageAdapter GetAdapter(string queueName, string queueRootPath)
        {
            return new MessageAdapter<T>(queueName, queueRootPath);
        }
    }
}
