using System.ComponentModel.DataAnnotations;

namespace LogAnalyticsViewer.Model.Entities
{
    public class Query
    {
        [Key]
        public int QueryId { get; set; }
        public string QueryText { get; set; }
    }
}
