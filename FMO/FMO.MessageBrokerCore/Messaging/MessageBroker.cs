using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public delegate void MessageEventHandler<T>(object sender, MessageEventArgs<T> e);

    /// <summary>
    /// Concrete implementation of the IMessageBroker interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(message.QueueName, message.QueueRootpath);

            adpt.PushMessage(message);

        }

        public T ReceiveMessage(string queueName, string queueRootPath)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            return adpt.PopMessage();
        }

        public bool HasMessage(string queueName, string queueRootPath)
        {

            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            return adpt.HasMessage();
        }

        public void Start(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            adpt.Start(handler);
        }

        public void Stop(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            adpt.Stop(handler);
        }
    }
}
