using System;

namespace LJS.GroupBetLogger.Api.Logging
{
    public interface ILogger
    {
        void Error(string message, Exception exception);
        void Information(string message);
        void Warning(string message);
    }
}