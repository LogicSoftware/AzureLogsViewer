using System.ComponentModel.DataAnnotations;

namespace LogAnalyticsViewer.Model.Entities
{
    public class DumpSettings
    {
        public DumpSettings()
        {
            DelayBetweenDumpsInMinutes = 15;
            DumpOverlapInMinutes = 5;
            DumpSizeInMinutes = 30;
        }

        [Key]
        public int Id { get; set; }

        public int DumpOverlapInMinutes { get; set; }

        public int DumpSizeInMinutes { get; set; }

        public int DelayBetweenDumpsInMinutes { get; set; }
    }
}
