using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace AzureLogsViewer.Tests.Helpers
{
    public class TimeComparer : IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            x = new DateTime(x.Year, x.Month, x.Day, x.Hour, x.Minute, x.Second);
            y = new DateTime(y.Year, y.Month, y.Day, y.Hour, y.Minute, y.Second);

            return x.CompareTo(y);
        }
    }

    public static class TimeComparerExtensions
    {
        public static CollectionItemsEqualConstraint WithinSeconds(this CollectionItemsEqualConstraint source)
        {
            return source.Using(new TimeComparer());
        }
    }
}