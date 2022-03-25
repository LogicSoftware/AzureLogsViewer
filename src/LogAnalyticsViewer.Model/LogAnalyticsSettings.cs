using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Rest.Azure.Authentication;

namespace LogAnalyticsViewer.Model
{
    public class LogAnalyticsSettings
    {       
        public string WorkspaceId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }

        public ActiveDirectoryServiceSettings AdSettings => new ActiveDirectoryServiceSettings
        {
            AuthenticationEndpoint = new Uri("https://login.microsoftonline.com"),
            TokenAudience = new Uri("https://api.loganalytics.io/"),
            ValidateAuthority = true
        };
    }
}
