using System.ComponentModel.DataAnnotations;

namespace LogAnalyticsViewer.Model.Entities
{
    public class LogAnalyticsSettings
    {
        [Key]
        public int Id { get; set; }

        public string WorkspaceId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
    }
}
