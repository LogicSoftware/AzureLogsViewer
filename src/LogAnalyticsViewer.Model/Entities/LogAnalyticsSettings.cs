using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogAnalyticsViewer.Model.Entities
{
    public class LogAnalyticsSettings
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string WorkspaceId { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string ClientId { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required]
        public string ClientSecret { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string Domain { get; set; }
    }
}
