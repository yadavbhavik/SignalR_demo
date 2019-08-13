using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Logging
{
    public interface INLogger
    {
        void WriteLogIntoFile(string MethodName, string Controllername, string LogData = null, string accessToken = null);
        void WriteErrorLog(string MethodName, string Controllername, Exception Error, string accessToken = null)
    }
}
