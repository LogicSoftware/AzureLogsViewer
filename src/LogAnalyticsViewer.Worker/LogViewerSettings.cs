namespace LogAnalyticsViewer.Worker
{
    public class LogViewerSettings
    {
        public int DumpSizeInMinutes { get; set; } = 11;
        public int DelayBetweenDumpsInMinutes { get; set; } = 7;
    }
}
