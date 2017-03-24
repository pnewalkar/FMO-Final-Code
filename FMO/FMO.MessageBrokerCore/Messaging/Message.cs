using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.Messaging
{
    public class Message:IMessage
    {
        private object _content;
        private DateTime _createdDate;
        private Guid _id;
        private long _priority;
        private MessageType _messageType;

        public Message()
        {
            _createdDate = DateTime.Now;
            _id = Guid.NewGuid();
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
        }

        public long Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        public object Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content=value;
            }
        }
        public MessageType MessType
        {
            get
            {
                return _messageType;
            }
            set
            {
                _messageType = value;
            }
        }


    }
}
