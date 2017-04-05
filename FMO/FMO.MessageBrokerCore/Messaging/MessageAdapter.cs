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
    //public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs args);
    internal class MessageAdapter<T> : IMessageAdapter<T>
    {
        //here is where we implement the message queue technology of our choice
        private MessageQueue messageQueue = null;
        public event EventHandler<MessageEventArgs<T>> MessageReceived;
        private bool _listen;

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

 
        public T PopMessage()
        {
            // Set the formatter to indicate body contains an Order.
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            System.Messaging.Message mqMessage = messageQueue.Receive();
            var serializer = new XmlSerializer(typeof(T));
            T returnMessage;

            using (TextReader reader = new StringReader((string)mqMessage.Body))
            {
                returnMessage = (T)serializer.Deserialize(reader);
            };

            // messageQueue.Purge();
            return returnMessage;
        }

        public void PushMessage(IMessage message)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
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

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            System.Messaging.Message msg = messageQueue.EndReceive(e.AsyncResult);

            StartListening();

            //string text = Encoding.Unicode.GetString(msg.Body);

            FireRecieveEvent(msg.Body);
        }

        private void OnPeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            messageQueue.EndPeek(e.AsyncResult);
            MessageQueueTransaction trans = new MessageQueueTransaction();
            System.Messaging.Message msg = null;
            try
            {
                trans.Begin();
                msg = messageQueue.Receive(trans);
                trans.Commit();

                StartListening();

                FireRecieveEvent((string)msg.Body);
            }
            catch
            {
                trans.Abort();
            }
        }

        private void FireRecieveEvent(object body)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageEventArgs<T>(body));
            }
        }

        public void Start(EventHandler<MessageEventArgs<T>> handler)
        {
            _listen = true;

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            messageQueue.PeekCompleted += new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            MessageReceived += handler;

            StartListening();
        }

        public void Stop(EventHandler<MessageEventArgs<T>> handler)
        {
            _listen = false;
            messageQueue.PeekCompleted -= new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(OnReceiveCompleted);
            MessageReceived -= handler;
        }

        private void StartListening()
        {
            if (!_listen)
            {
                return;
            }

            // The MSMQ class does not have a BeginRecieve method that can take in a 
            // MSMQ transaction object. This is a workaround – we do a BeginPeek and then 
            // recieve the message synchronously in a transaction.
            // Check documentation for more details
            if (messageQueue.Transactional)
            {
                messageQueue.BeginPeek();
            }
            else
            {
                messageQueue.BeginReceive();
            }
        }
    }
}
