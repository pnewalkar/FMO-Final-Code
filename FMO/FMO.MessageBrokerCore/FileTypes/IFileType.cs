using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.MessageBrokerCore.FileTypes
{
    interface IFileType
    {
        KeyValuePair<string, string> GetQueue(string rootPath);
    }
}
