using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogAnalyticsViewer.Model.Entities
{
    public class Query
    {
        public Query()
        {
            Events = new HashSet<Event>();
        }
        [Key]
        public int QueryId { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string DisplayName { get; set; }

        [Required]
        public string QueryText { get; set; }

        public bool Enabled { get; set; } = true;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Channel { get; set; }
        
        public ICollection<Event> Events { get; set; }
    }
}
