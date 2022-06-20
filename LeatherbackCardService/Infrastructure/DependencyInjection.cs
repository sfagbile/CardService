using System;
using System.Net.Http.Headers;
using System.Text;
using Domain.Interfaces;
using Infrastructure.Infrastructure;
using Infrastructure.RailsIntegration;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient<IRailsBankService, RailsBankService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("RailsBank").GetSection("BaseUrl").Value);
                client.DefaultRequestHeaders.Add("Authorization", $"{configuration.GetSection("RailsBank").GetSection("ApiKey").Value}"); 
            });
            
            services.AddHttpClient<IOtpHttpService, OtpHttpService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("OtpService").GetSection("BaseUrl").Value);
            });

            services.AddHttpClient<IMeaWalletService, MeaWalletService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MeaWallet:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Mea-Api-Key-Id", $"{configuration["MeaWallet:MeaApiKey"]}");
            });
            
            if (configuration.GetValue<bool>("UseAzureSignalRService"))
            {
                services.AddSignalR(x =>
                {
                    x.EnableDetailedErrors = true;
                }).AddAzureSignalR(options =>
                {
                    options.ConnectionCount = 1;
                    options.Endpoints = new ServiceEndpoint[]
                    {
                        new(configuration.GetSection("SignalRConnectionStrings").GetSection("PrimaryConnectionString").Value),
                        new(configuration.GetSection("SignalRConnectionStrings").GetSection("SecondaryConnectionString").Value, EndpointType.Secondary)
                    };
                }).AddJsonProtocol();
            }
            else
            {
                services.AddSignalR();
            }
            
            return services;
        }
    }
}