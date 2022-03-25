namespace LogAnalyticsViewer.Model.DTO
{
    public class SaveQueryModel
    {
        public int QueryId { get; set; }

        public string DisplayName { get; set; }

        public string QueryText { get; set; }

        public bool Enabled { get; set; } = true;

        public string Channel { get; set; }
    }
}
