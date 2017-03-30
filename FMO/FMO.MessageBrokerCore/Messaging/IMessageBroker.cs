using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public enum MessageType
    {
        PostalAddress,
        NotYetBuilt,
        AddressLocation,
        ReceivedMessage,
        ThirdParty
    }

    public interface IMessageBroker<T>
    {

        //takes an object, serialises it and returns a message
        IMessage CreateMessage(Object obj, string queueName, string queueRootPath);

        //sends a message to the specified recipient
        void SendMessage(IMessage message);

        //Gets a message 
        IMessage ReceiveMessage(string queueName, string queueRootPath);

        bool HasMessage(string queueName, string queueRootPath);

    }
}
