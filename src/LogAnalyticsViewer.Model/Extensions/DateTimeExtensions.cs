using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyticsViewer.Model.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToAzureDate(this DateTime date)
        {
            return @$"datetime(""{ date.ToString("yyyy-MM-dd")}"")";
        }
    }
}
