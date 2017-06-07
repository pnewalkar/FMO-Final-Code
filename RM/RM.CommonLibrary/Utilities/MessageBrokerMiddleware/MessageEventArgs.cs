using System;
using System.IO;
using System.Xml.Serialization;

namespace RM.CommonLibrary.MessageBrokerMiddleware
{
    /// <summary>
    /// Custom implementation of the Event Args for Message events. This istriggered when there is a message available on the queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageEventArgs<T> : EventArgs
    {
        private T _messageBody;

        public T MessageBody
        {
            get { return _messageBody; }
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="body"></param>
        public MessageEventArgs(object body)
        {
            var serializer = new XmlSerializer(typeof(T));
            T returnMessage;

            using (TextReader reader = new StringReader((string)body))
            {
                returnMessage = (T)serializer.Deserialize(reader);
            };
            _messageBody = returnMessage;
        }
    }
}