using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LogAnalyticsViewer.Model.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToAzureDate(this DateTime date)
        {
            return @$"datetime(""{ date.ToCommonFormat()}"")";
        }

        public static string ToCommonFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }
    }
}
