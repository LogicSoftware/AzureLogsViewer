namespace LogAnalyticsViewer.Model.DTO
{
    public class CreateQueryModel
    {
        public string DisplayName { get; set; }

        public string QueryText { get; set; }

        public bool Enabled { get; set; } = true;

        public string Channel { get; set; }
    }
}
