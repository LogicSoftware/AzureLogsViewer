namespace AzureLogsViewer.Model.Entities
{
    public class SlackMessage
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Chanel { get; set; }

        public string WebHookUrl { get; set; }
    }
}