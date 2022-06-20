using System.Reflection;
using ApplicationServices.AutomapperConfiguration;
using ApplicationServices.Card.Processors;
using ApplicationServices.CardIssuance.Processors;
using ApplicationServices.CardManagement.Processors;
using ApplicationServices.Common;
using ApplicationServices.Common.Behaviour;
using ApplicationServices.Common.Options;
using ApplicationServices.Customer.Processors;
using ApplicationServices.Customer.Processors.RailsBankProcessor;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Card;
using ApplicationServices.Interfaces.CardDetailServices;
using ApplicationServices.Interfaces.Transaction;
using ApplicationServices.Services;
using ApplicationServices.TransactionNotification.Processors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationServices
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RailsBankRoot>(configuration.GetSection("RailsBank"));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnHandledExceptionBehaviour<,>));
            
            services.AddScoped<ICardDetailService, RailsBankCardDetailProcessor>();
            services.AddScoped<ICardIssuanceService, RailsBankCardIssuerProcessor>();
            services.AddScoped<ITransactionNotificationService, RailsBankTransactionNotificationProcessor>();
            services.AddScoped<ICardManagementService, RailsBankCardManagementServiceProcessor>();
            services.AddScoped<ICardPinService, CardPinService>();
            services.AddScoped<IProviderEndUserService, RailsBankProviderEndUserProcessor>();
            services.AddScoped<ICardService, RailsBankGetCardDetailsProcessor>();
            services.AddScoped<IOtpService, OtpService>();
            

            services.AddScoped<IStrategyResolver<ICardDetailService>, StrategyResolver<ICardDetailService>>();
            services.AddScoped<IStrategyResolver<ICardIssuanceService>, StrategyResolver<ICardIssuanceService>>();
            services.AddScoped<IStrategyResolver<ITransactionNotificationService>, StrategyResolver<ITransactionNotificationService>>();
            services.AddScoped<IStrategyResolver<ICardManagementService>, StrategyResolver<ICardManagementService>>();
            services.AddScoped<IStrategyResolver<IProviderEndUserService>, StrategyResolver<IProviderEndUserService>>();
            services.AddScoped<IStrategyResolver<ICardService>, StrategyResolver<ICardService>>();

            return services;
        }
    }
}