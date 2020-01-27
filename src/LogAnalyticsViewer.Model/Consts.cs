using Microsoft.Rest.Azure.Authentication;
using System;

namespace LogAnalyticsViewer.Model
{
    public class Consts
    {
        static readonly string authEndpoint = "https://login.microsoftonline.com";
        static readonly string tokenAudience = "https://api.loganalytics.io/";

        public static ActiveDirectoryServiceSettings AdSettings => new ActiveDirectoryServiceSettings
        {
            AuthenticationEndpoint = new Uri(authEndpoint),
            TokenAudience = new Uri(tokenAudience),
            ValidateAuthority = true
        };
    }
}
