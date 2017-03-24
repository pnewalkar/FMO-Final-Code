using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Messaging; //This is the only place where we reference MSMQ
using System.Configuration;
using Fmo.MessageBrokerCore.FileTypes;

namespace Fmo.MessageBrokerCore.Messaging
{
    internal class MessageAdapter : IMessageAdapter
    {
        //here is where we implement the message queue technology of our choice
        private MessageQueue messageQueue = null;
        private static string QueueRootPath = ConfigurationSettings.AppSettings["Queue_Root_Path"].ToString();

        public MessageAdapter(IFileType fileType)
        {

            KeyValuePair<string, string> queueDetails = fileType.GetQueue(QueueRootPath);
            //TODO: Route the message to different queue depending on messagetype
            if (MessageQueue.Exists(queueDetails.Value))
            {
                messageQueue = new MessageQueue(queueDetails.Value);
                messageQueue.Label = queueDetails.Key;

            }
            else
            {
                // Create the Queue
                MessageQueue.Create(queueDetails.Value);
                messageQueue = new MessageQueue(queueDetails.Value);
                messageQueue.Label = queueDetails.Key;
            }
        }

 
        public IMessage PopMessage()
        {
            // Set the formatter to indicate body contains an Order.
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Message) });
            System.Messaging.Message mqMessage = messageQueue.Receive();
            Message returnMessage = (Message)mqMessage.Body;

           // messageQueue.Purge();
            return returnMessage;
        }

        public void PushMessage(IMessage message)
        {
            messageQueue.Send(message);
        }

        public bool HasMessage()
        {
            return messageQueue.GetAllMessages().Count() > 0;
        }


    }
}
