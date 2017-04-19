using System;

namespace Fmo.MessageBrokerCore.Messaging
{
    /// <summary>
    /// IMessageAdapter is used to provide the core functionality of a message queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageAdapter<T>
    {
        event EventHandler<MessageEventArgs<T>> MessageReceived;

        /// <summary>
        /// Pull the message out of the queue
        /// </summary>
        /// <returns></returns>
        T PopMessage();


        /// <summary>
        /// Add message to the queue
        /// </summary>
        /// <param name="message"></param>
        void PushMessage(IMessage message);


        /// <summary>
        /// Check whether the queue has messages.
        /// </summary>
        /// <returns></returns>
        bool HasMessage();

        /// <summary>
        /// Start method to register the handlers and set the formatter and start listening for queue
        /// messages.
        /// </summary>
        /// <param name="handler"></param>
        void Start(EventHandler<MessageEventArgs<T>> handler);

        /// <summary>
        /// Start Listening for the queue messages.
        /// </summary>
        void Stop(EventHandler<MessageEventArgs<T>> handler);
    }
}


