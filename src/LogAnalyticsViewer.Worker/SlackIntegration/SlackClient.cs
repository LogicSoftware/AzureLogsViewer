using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    //A simple C# class to post messages to a Slack channel
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class SlackClient
    {
        private readonly HttpClient _httpClient;
        public SlackClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task PostMessage(string token, string text, string channel)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = null,
                Text = text
            };

            await PostMessage(token, payload);
        }

        private async Task PostMessage(string token, Payload payload)
        {
            var data = new
            {
                username = "azure-logs",
                channel = payload.Channel,
                text = payload.Text,
                link_names = true,
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            var response = await _httpClient.PostAsJsonAsync($"https://slack.com/api/chat.postMessage", data);

            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiRespBase>();
            if (!apiResponse.Ok)
            {
                throw new Exception(apiResponse.Error);
            }
        }
        
        
        
    }
    
    public class ApiRespBase
    {
        public bool Ok { get; set; }

        public string Error { get; set; }
    }
}