using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public class MessageBroker<T> : IMessageBroker<T>
    {
        private MessageAdapterFactory<T> msgAdapterFactory;

        public MessageBroker()
        {
            msgAdapterFactory = new MessageAdapterFactory<T>();
        }

        public IMessage CreateMessage(Object obj, string queueName, string queueRootPath)
        {
            Message msg = new Message();
            msg.Content = obj;
            msg.QueueName = queueName;
            msg.QueueRootpath = queueRootPath;
            return msg; 
        }
        public void SendMessage(IMessage message)
        {

            IMessageAdapter adpt = msgAdapterFactory.GetAdapter(message.QueueName, message.QueueRootpath);

            adpt.PushMessage(message);

        }

        public IMessage ReceiveMessage(string queueName, string queueRootPath)
        {
            IMessageAdapter adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            return adpt.PopMessage();
        }

        public bool HasMessage(string queueName, string queueRootPath)
        {

            IMessageAdapter adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            return adpt.HasMessage();
        }

       
    }
}
