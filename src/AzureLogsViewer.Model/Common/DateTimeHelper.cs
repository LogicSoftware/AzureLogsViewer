using System;

namespace AzureLogsViewer.Model.Common
{
    public static class DateTimeHelper
    {
        public static DateTime Min(DateTime first, DateTime second)
        {
            return first < second ? first : second;
        }
    }
}