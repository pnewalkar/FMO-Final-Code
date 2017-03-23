using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.FileTypes
{
    public class PAFFileType : IFileType
    {
        StringBuilder m_QueuePath;

        public KeyValuePair<string, string> GetQueue(string rootPath)
        {
            m_QueuePath = new StringBuilder();
            m_QueuePath.Append(rootPath);
            m_QueuePath.Append(Constants.QUEUE_PAF);
            return new KeyValuePair<string, string>(Constants.QUEUE_PAF, m_QueuePath.ToString());
        }
    }
}
