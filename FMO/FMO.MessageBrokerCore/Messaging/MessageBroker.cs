using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public class MessageBroker : IMessageBroker
    {
        private MessageAdapterFactory msgAdapterFactory;

        public MessageBroker()
        {
            msgAdapterFactory = new MessageAdapterFactory();
        }

        public IMessage CreateMessage(Object obj, MessageType msgType)
        {
            Message msg = new Message();
            msg.Content = obj;
            msg.MessType = msgType;
            return msg; 
        }
        public void SendMessage(IMessage message)
        {

            IMessageAdapter adpt = msgAdapterFactory.GetAdapter(message.MessType);

            adpt.PushMessage(message);

        }

        public IMessage ReceiveMessage(MessageType type)
        {
            IMessageAdapter adpt = msgAdapterFactory.GetAdapter(type);

            return adpt.PopMessage();
        }

        public bool HasMessage(MessageType type)
        {
            IMessageAdapter adpt = msgAdapterFactory.GetAdapter(type);

            return adpt.HasMessage();
        }

       
    }
}
