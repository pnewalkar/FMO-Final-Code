using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    /// <summary>
    /// IMessageAdapter is used to provide the core functionality of a message queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageAdapter<T>
    {
        event EventHandler<MessageEventArgs<T>> MessageReceived;
        T PopMessage();

        void PushMessage(IMessage message);

        bool HasMessage();

        void Start(EventHandler<MessageEventArgs<T>> handler);

        void Stop(EventHandler<MessageEventArgs<T>> handler);
    }
}


