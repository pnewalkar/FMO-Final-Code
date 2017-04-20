using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Interface
{
    public interface ILoggingHelper
    {
        void LogError(Exception exception);

        void LogError(string message, Exception exception);

        void LogInfo(string message);

        void LogInfo(string message, bool enableLogging);

        void LogWarn(string message);
    }
}
