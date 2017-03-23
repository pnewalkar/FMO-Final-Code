using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public interface IMessageAdapter
    {
        IMessage PopMessage();

        void PushMessage(IMessage message);

        bool HasMessage();
    }
}
