using System;
using NLog;

namespace AzureLogsViewer.Model.Infrastructure
{
    public class AlvLogger
    {
        public static void LogError(string message, Exception ex)
        {
            LogManager.GetLogger("Alv").Error(message, ex);
        }
    }
}