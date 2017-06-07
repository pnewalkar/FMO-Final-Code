namespace RM.CommonLibrary.MessageBrokerMiddleware
{
    /// <summary>
    /// Assists in creation of the Message Adapter instance. This is used by the client code to create an instance of the Message Adapter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MessageAdapterFactory<T>
    {
        /// <summary>
        /// Provides an adapter for the queueing technology that we are currently using
        /// </summary>
        /// <param name="queueName">Queue name </param>
        /// <param name="queueRootPath">Queue path</param>
        /// <returns></returns>
        internal IMessageAdapter<T> GetAdapter(string queueName, string queueRootPath)
        {
            return new MessageAdapter<T>(queueName, queueRootPath);
        }
    }
}