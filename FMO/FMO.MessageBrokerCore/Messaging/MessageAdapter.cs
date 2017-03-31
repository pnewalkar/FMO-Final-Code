using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Messaging; //This is the only place where we reference MSMQ
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace Fmo.MessageBrokerCore.Messaging
{
    internal class MessageAdapter<T> : IMessageAdapter
    {
        //here is where we implement the message queue technology of our choice
        private MessageQueue messageQueue = null;

        public MessageAdapter(string queueName, string queueRootPath)
        {

            StringBuilder queueBuilder = new StringBuilder();

            queueBuilder.Append(queueRootPath);
            queueBuilder.Append(queueName);

            //KeyValuePair<string, string> queueDetails = fileType.GetQueue(QueueRootPath);
            //TODO: Route the message to different queue depending on messagetype
            if (MessageQueue.Exists(queueBuilder.ToString()))
            {
                messageQueue = new MessageQueue(queueBuilder.ToString());
                messageQueue.Label = queueName;

            }
            else
            {
                // Create the Queue
                MessageQueue.Create(queueBuilder.ToString());
                messageQueue = new MessageQueue(queueBuilder.ToString());
                messageQueue.Label = queueName;
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
            XmlSerializer xsSubmit = new XmlSerializer(typeof(string));
            var subReq = message.Content;
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }

            messageQueue.Send(xml);
        }

        public bool HasMessage()
        {
            return messageQueue.GetAllMessages().Count() > 0;
        }


    }
}
