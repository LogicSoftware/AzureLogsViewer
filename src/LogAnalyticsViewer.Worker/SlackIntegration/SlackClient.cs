/*
 * Get this code here https://gist.github.com/jogleasonjr/7121367
 */

using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    //A simple C# class to post messages to a Slack channel
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class SlackClient
    {
        private readonly Encoding _encoding = new UTF8Encoding();

        //Post a message using simple strings
        public void PostMessage(string urlWithAccessToken, string text, string channel)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = null,
                Text = text
            };

            PostMessage(urlWithAccessToken, payload);
        }

        //Post a message using a Payload object
        private void PostMessage(string urlWithAccessToken, Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(new Uri(urlWithAccessToken), "POST", data);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }
    }
}