using System;
using GreenPipes;
using MassTransit;
using MessageBus.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageBus.Extensions
{
    internal static class ConfigureRabbitMqOverMasstransit
    {
        internal static IServiceCollection ConfigureBus(this IServiceCollection services, IConfiguration config)
        {
            var username = config["RabbitMQ:Username"];
            var password = config["RabbitMQ:Password"];
            var rabbitMqUrl = config["RabbitMQ:Url"];

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CardRequestSagaConsumer>();
                x.AddConsumer<CardActivationConsumer>();
                x.AddConsumer<TransactionNotificationConsumer>();
                x.AddConsumer<CreateCardRequestConsumer>();
                
                
                
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(rabbitMqUrl, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("CardRequestQueue", e =>
                    {
                        e.Durable = true;
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(3, 2000));
                        e.UseCircuitBreaker(cb =>
                        {
                            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                            cb.TripThreshold = 15;
                            cb.ActiveThreshold = 10;
                            cb.ResetInterval = TimeSpan.FromMinutes(3);
                        });
                        e.Consumer<CardRequestSagaConsumer>(provider);
                        e.DiscardSkippedMessages();
                        e.DiscardFaultedMessages();
                    });
                    
                    cfg.ReceiveEndpoint("CardActivationQueue", consumer =>
                    {
                        consumer.Durable = true;
                        consumer.ConfigureConsumer<CardActivationConsumer>(provider);
                    });
                
                    
                    cfg.ReceiveEndpoint("TransactionNotificationCardQueue", consumer =>
                    {
                        consumer.Durable = true;
                        consumer.ConfigureConsumer<TransactionNotificationConsumer>(provider);
                    });

                    cfg.ReceiveEndpoint("CreateCardRequestQueue", consumer =>
                    {
                        consumer.Durable = true;
                        consumer.ConfigureConsumer<CreateCardRequestConsumer>(provider);
                    });
                    
                }));
            });
            
            services.AddSingleton<IHostedService, BusService>();
            return services;
        }
    }
}