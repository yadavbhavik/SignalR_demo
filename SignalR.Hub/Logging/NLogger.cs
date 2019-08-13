using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Logging
{
    public class NLogger
    {
        public static void WriteLogIntoFile(string MethodName, string Controllername, string LogData = null, string accessToken = null)        {            var logger = NLog.LogManager.GetCurrentClassLogger();            try            {                logger.Info("DateTime:" + DateTime.UtcNow + "##MethodName:" + MethodName + "##Controllername: " + Controllername + Environment.NewLine + "LogData: " + LogData + Environment.NewLine + "===================================================================================================================");            }            catch (Exception ex)            {                logger.Error(ex);            }        }        public static void WriteErrorLog(string MethodName, string Controllername, Exception Error, string accessToken = null)        {            var logger = NLog.LogManager.GetCurrentClassLogger();            try            {                logger.Error("DateTime:" + DateTime.UtcNow + "##MethodName:" + MethodName + "##Controllername: " + Controllername + Environment.NewLine + "Error: " + Error + Environment.NewLine + "===================================================================================================================");            }            catch (Exception ex)            {                logger.Error(ex);            }        }
    }
}
