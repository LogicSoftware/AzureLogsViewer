using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.DTO
{
    public class SlackIntegrationInfoModel
    {
        public SlackIntegrationInfoModel()
        {
            Filter = new WadLogsFilter();
        }

        public SlackIntegrationInfoModel(SlackIntegrationInfo entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            WebHookUrl = entity.WebHookUrl;
            Chanel = entity.Chanel;
            MessagePattern = entity.MessagePattern;
            Filter = entity.GetFilter();
        }

        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string WebHookUrl { get; set; }

        public string Chanel { get; set; }

        public string MessagePattern { get; set; }

        public WadLogsFilter Filter { get; set; }

        public bool IsPersisted()
        {
            return Id != 0;
        }

        public void ApplyChanges(SlackIntegrationInfo entity)
        {
            entity.Enabled = Enabled;
            entity.WebHookUrl = WebHookUrl;
            entity.Chanel = Chanel;
            entity.MessagePattern = MessagePattern;
            entity.SetFilter(Filter);
        }
    }
}