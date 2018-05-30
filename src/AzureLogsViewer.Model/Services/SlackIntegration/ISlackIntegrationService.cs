using System.Collections.Generic;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.Services.SlackIntegration
{
    public interface ISlackIntegrationService
    {
        void ProcessLogEntries(List<WadLogEntry> entries);
        void SendMessages();
        IEnumerable<SlackIntegrationInfoModel> GetIntegrationInfos();
        SlackIntegrationInfoModel Save(SlackIntegrationInfoModel model);
    }
}