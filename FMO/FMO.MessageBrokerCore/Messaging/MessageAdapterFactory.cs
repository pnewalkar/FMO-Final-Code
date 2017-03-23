using Fmo.MessageBrokerCore.FileTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
                //    //MessageQueue messageQueue = null;
    internal class MessageAdapterFactory
    {
        //provides an adapter for the queueing technology that we are currently using
        internal IMessageAdapter GetAdapter(MessageType type)
        {
            IFileType fileType = null;

            switch (type)
            {
                case MessageType.NotYetBuilt:
                    fileType = new NYBFileType();
                    break;
                case MessageType.PostalAddress:
                    fileType = new PAFFileType();
                    break;
                case MessageType.ThirdParty:
                    fileType = new ThirdPartyFileType();
                    break;

            }
            IMessageAdapter objAdapter=new  MessageAdapter(fileType);
            return objAdapter;

        }
    }
}
