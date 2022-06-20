using System;
using System.Net.Http;
using ApplicationServices.Common.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Infrastructure
{
    public class RailsBankHttpClient
    {
        public HttpClient Client { get; set; }

        public RailsBankHttpClient(HttpClient client , IOptionsMonitor<RailsBankRoot> railsBank)
        {
            Client = client;
            var railsBankRoot = railsBank.CurrentValue;
            Client.BaseAddress = new Uri(railsBankRoot.BaseUrl);
            client.DefaultRequestHeaders.Add("Authorization", railsBankRoot.ApiKey);
            // client.DefaultRequestHeaders.Authorization = 
            //     new AuthenticationHeaderValue("API-KEY", configuration1.GetSection("RailsBank").GetSection("ApiKey").Value);  
        }
    }
}