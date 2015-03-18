using System;
using System.Collections.Generic;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.WadLogs
{
    public interface IIWadLogsReader
    {
        List<WadLogEntry> Read(DateTime from, DateTime to);
    }
}