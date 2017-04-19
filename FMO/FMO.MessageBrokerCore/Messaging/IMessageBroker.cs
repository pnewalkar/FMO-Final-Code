using System;

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

    /// <summary>
    /// IMessageBroker, exposes the Message queue functionality to the client.
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public interface IMessageBroker<T>
    {

        /// <summary>
        /// takes an object, serialises it and returns a message
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns>MSMQ message</returns>
        IMessage CreateMessage(Object obj, string queueName, string queueRootPath);

        /// <summary>
        /// Add message to queue
        /// </summary>
        /// <param name="message">MSMQ message</param>
        void SendMessage(IMessage message);

        /// <summary>
        /// Read message from queue
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns>serialized object</returns>
        T ReceiveMessage(string queueName, string queueRootPath);

        /// <summary>
        /// Start listening for in coming message
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <param name="handler">Message receieved handler</param>
        void Start(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler);

        /// <summary>
        /// Stops listening to MSMQ
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <param name="handler">Message receieved handler</param>
        void Stop(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler);

        /// <summary>
        /// Checks for message in MSMQ
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns>boolean value true if message exists in queue</returns>
        bool HasMessage(string queueName, string queueRootPath);


    }
}
