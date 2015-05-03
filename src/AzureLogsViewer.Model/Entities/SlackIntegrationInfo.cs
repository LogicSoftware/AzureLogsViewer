using AzureLogsViewer.Model.DTO;
using Newtonsoft.Json;

namespace AzureLogsViewer.Model.Entities
{
    public class SlackIntegrationInfo
    {
        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string WebHookUrl { get; set; }

        public string Chanel { get; set; }

        public string SerializedFilter { get; set; }

        public string MessagePattern { get; set; }

        public WadLogsFilter GetFilter()
        {
            if(string.IsNullOrWhiteSpace(SerializedFilter))
                return new WadLogsFilter();

            return JsonConvert.DeserializeObject<WadLogsFilter>(SerializedFilter);
        }

        public void SetFilter(WadLogsFilter filter)
        {
            SerializedFilter = JsonConvert.SerializeObject(filter);
        }
    }
}