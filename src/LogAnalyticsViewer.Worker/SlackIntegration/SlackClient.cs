using System;
using RestSharp;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    //A simple C# class to post messages to a Slack channel
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class SlackClient
    {
        private RestClient _apiClient = new RestClient("https://slack.com/api/");

        public void PostMessage(string token, string text, string channel)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = null,
                Text = text
            };

            PostMessage(token, payload);
        }

        private void PostMessage(string token, Payload payload)
        {
            var data = new
            {
                username = "azure-logs",
                channel = payload.Channel,
                text = payload.Text,
                link_names = true,
            };
            var request = new RestRequest("chat.postMessage");
            request.AddParameter("token", token);
            request.AddObject(data);
            var response = _apiClient.Post<ApiRespBase>(request);
            CheckResponse(response);
        }

        private void CheckResponse<T>(IRestResponse<T> response)
            where T : ApiRespBase
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.ErrorException != null)
            {
                var message = "Error during posting message to slack. " + Environment.NewLine +
                    "StatusCode: " + response.StatusCode + Environment.NewLine +
                    "ErrorException: " + response.ErrorException != null ? response.ErrorException.ToString() : " - ";
                throw new Exception(message);
            }

            var respData = response.Data;
            if (!respData.ok)
            {
                throw new Exception($"slack responded with failed response: {respData.error}");
            }
        }


        public class ApiRespBase
        {
            public bool ok { get; set; }

            public string error { get; set; }
        }
    }
}