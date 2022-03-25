namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    public class SlackIntegrationSettings
    {        
        public string EventUrlFormat { get; set; }
        public string ApiToken { get; set; }

        public int RatePerQuery { get; set; } = 100;
        public int MessageLength { get; set; } = 350;
        public string MessagePattern { get; set; } = @"{Date} - {Source}
{Message}
occured {Total} times
see details {Link}
__________________________";
    }
}
