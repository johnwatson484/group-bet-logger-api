using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LJS.GroupBetLogger.Api.Logging
{
    public class Logger : ILogger
    {
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Warning(string message)
        {
            log.Warn(message);
        }

        public void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Information(string message)
        {
            log.Info(message);
        }
    }
}