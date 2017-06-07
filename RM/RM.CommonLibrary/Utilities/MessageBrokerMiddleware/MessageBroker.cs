using System;

namespace RM.CommonLibrary.MessageBrokerMiddleware
{
    public delegate void MessageEventHandler<T>(object sender, MessageEventArgs<T> e);

    /// <summary>
    /// Concrete implementation of the IMessageBroker interface. The Message Broker acts as a wrapper over the Message Queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageBroker<T> : IMessageBroker<T>
    {
        private MessageAdapterFactory<T> msgAdapterFactory;

        public MessageBroker()
        {
            msgAdapterFactory = new MessageAdapterFactory<T>();
        }

        /// <summary>
        /// takes an object, serialises it and returns a message
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns>MSMQ message</returns>
        public IMessage CreateMessage(Object obj, string queueName, string queueRootPath)
        {
            Message msg = new Message();
            msg.Content = obj;
            msg.QueueName = queueName;
            msg.QueueRootpath = queueRootPath;
            return msg;
        }

        /// <summary>
        /// Add message to queue
        /// </summary>
        /// <param name="message">MSMQ message</param>
        public void SendMessage(IMessage message)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(message.QueueName, message.QueueRootpath);

            adpt.PushMessage(message);
        }

        /// <summary>
        /// Read message from queue
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns>serialized object</returns>
        public T ReceiveMessage(string queueName, string queueRootPath)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            return adpt.PopMessage();
        }

        /// <summary>
        /// Checks for message in MSMQ
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns>boolean value true if message exists in queue</returns>
        public bool HasMessage(string queueName, string queueRootPath)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            return adpt.HasMessage();
        }

        /// <summary>
        /// Start listening for in coming message
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <param name="handler">Message receieved handler</param>
        public void Start(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            adpt.Start(handler);
        }

        /// <summary>
        /// Stops listening to MSMQ
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <param name="handler">Message receieved handler</param>
        public void Stop(string queueName, string queueRootPath, EventHandler<MessageEventArgs<T>> handler)
        {
            IMessageAdapter<T> adpt = msgAdapterFactory.GetAdapter(queueName, queueRootPath);

            adpt.Stop(handler);
        }
    }
}