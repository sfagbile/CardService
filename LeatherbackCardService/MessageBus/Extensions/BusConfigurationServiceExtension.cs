using System;
using System.Collections.Generic;
using ApplicationServices.Interfaces;
using MessageBus.Publishers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageBus.Extensions
{
    public static class BusConfigurationServiceExtension
    {
        public static IServiceCollection AddMessagingBus(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddTransient<IMessagePublisher, MessagePublisher>();
            services.ConfigureBus(env, config);
            return services;
        }
        
        private static void ConfigureBus(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            var dictionary = new Dictionary<string, Func<IServiceCollection, IConfiguration, IServiceCollection>>
            {
                { Environments.Development, ConfigureRabbitMqOverMasstransit.ConfigureBus},
                { Environments.Staging, ConfigureRabbitMqOverMasstransit.ConfigureBus},
                { Environments.Production, ConfigureRabbitMqOverMasstransit.ConfigureBus}
            };
            dictionary[env.EnvironmentName](services, config);
        }
    }
}