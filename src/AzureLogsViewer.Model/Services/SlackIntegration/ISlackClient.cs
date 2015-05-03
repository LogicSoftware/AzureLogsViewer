namespace AzureLogsViewer.Model.Services.SlackIntegration
{
    public interface ISlackClient
    {
        void PostMessage(string urlWithAccessToken, string text, string channel);
    }
}