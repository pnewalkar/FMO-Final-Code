using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
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


