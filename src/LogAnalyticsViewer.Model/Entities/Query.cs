using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogAnalyticsViewer.Model.Entities
{
    public class Query
    {
        [Key]
        public int QueryId { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string QueryText { get; set; }
    }
}
