using System;
using System.Collections.Generic;

namespace AzureLogsViewer.Model.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}