using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fmo.MessageBrokerCore.Messaging
{
    /// <summary>
    /// Custom implementation of the Event Args for Message events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageEventArgs<T> : EventArgs
    {
        private T _messageBody;

        public T MessageBody
        {
            get { return _messageBody; }
        }

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
