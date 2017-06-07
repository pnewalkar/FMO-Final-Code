using System;
using System.IO;
using System.Linq;
using System.Messaging; //This is the only place where we reference MSMQ
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.MessageBrokerMiddleware
{
    /// <summary>
    /// Implements the IMessageAdapter interface. The message adapter is the class used by the client code to interact with the Message Queue via the Message Broker.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
                messageQueue.SetPermissions(Constants.MSGQUEUEPERMISSION, MessageQueueAccessRights.FullControl);
                messageQueue.Label = queueName;
            }
        }

        /// <summary>
        /// Pull the message out of the queue
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Add message to the queue
        /// </summary>
        /// <param name="message"></param>
        public void PushMessage(IMessage message)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            var subReq = message.Content;
            var xml = "";

            //Serialize the message to xml
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }

            //Push message to MSMQ.
            messageQueue.Send(xml);
        }

        /// <summary>
        /// Check whether the queue has messages.
        /// </summary>
        /// <returns></returns>
        public bool HasMessage()
        {
            return messageQueue.GetAllMessages().Count() > 0;
        }

        /// <summary>
        /// Handler for the queue Receive Completed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            System.Messaging.Message msg = messageQueue.EndReceive(e.AsyncResult);

            StartListening();

            FireRecieveEvent(msg.Body);
        }

        /// <summary>
        /// Handler for the queue Peek Completed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Fire the custom event for a message received.
        /// </summary>
        /// <param name="body"></param>
        private void FireRecieveEvent(object body)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageEventArgs<T>(body));
            }
        }

        /// <summary>
        /// Start method to register the handlers and set the formatter and start listening for queue
        /// messages.
        /// </summary>
        /// <param name="handler"></param>
        public void Start(EventHandler<MessageEventArgs<T>> handler)
        {
            _listen = true;

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            messageQueue.PeekCompleted += new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            MessageReceived += handler;

            StartListening();
        }

        /// <summary>
        /// Stop method to de-register the handlers and stop listening to the queue for messages.
        /// </summary>
        /// <param name="handler"></param>
        public void Stop(EventHandler<MessageEventArgs<T>> handler)
        {
            _listen = false;
            messageQueue.PeekCompleted -= new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(OnReceiveCompleted);
            MessageReceived -= handler;
        }

        /// <summary>
        /// Start Listening for the queue messages.
        /// </summary>
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