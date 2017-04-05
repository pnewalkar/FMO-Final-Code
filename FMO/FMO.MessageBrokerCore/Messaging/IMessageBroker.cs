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
        T ReceiveMessage(string queueName, string queueRootPath);

        void Start(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler);

        void Stop(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler);

        bool HasMessage(string queueName, string queueRootPath);


    }
}
