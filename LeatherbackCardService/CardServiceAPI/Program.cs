using System;
using System.Threading.Tasks;
using LeatherBack.SharedLibrary.Logger.Extenstions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Context;
using Serilog;

namespace CardServiceAPI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        ConfigureLog(services);
                        Log.Information("Starting card service host........");
                        var context = services.GetRequiredService<CardServiceDbContext>();
                        if (context.Database.IsSqlServer())
                        {
                            await context.Database.MigrateAsync();
                            Log.Information("Database migration successful");
                        }

                        await Persistence.DbContextSeeds.DbContextSeed.SeedInitialAsync(context);
                        Log.Information("Running Seed Data....");
                    }
                    catch (Exception ex)
                    {
                        Log.Information("Error starting host: " + ex + "\n Message: " + ex.Message +
                                        "\n Inner Exception: " + ex.InnerException);
                        throw;
                    }
                    finally
                    {
                        Log.Information("Started card service host");
                    }
                }

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
                Log.Information("Started card service host");
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
                {
                    var env = hostBuilderContext.Configuration;
                    loggerConfiguration.ConfigureBaseLogging(services, env);
                    loggerConfiguration.AddElasticSearchLogging(services, env);
                    loggerConfiguration.AddApplicationInsightsLogging(services, env);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });


        private static void ConfigureLog(IServiceProvider serviceProvider)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();


            LeatherBackLoggerConfigurationExtensions.SetupLoggerConfiguration(serviceProvider, configuration);
            // var loggerConfiguration = new LoggerConfiguration()
            //     .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            //     .MinimumLevel.Override("Microsoft.AspNetCore.SignalR", LogEventLevel.Error) // Should be relevant to SignalR logging
            //     .MinimumLevel.Override("Microsoft.AspNetCore.Http.Connections", LogEventLevel.Error) // Should be relevant to SignalR logging
            //     .Enrich.FromLogContext()
            //     .Enrich.WithExceptionDetails()
            //     .Enrich.WithProperty("Company", "Leatherback")
            //     .Enrich.WithProperty("Application", "PaymentGateway-Api")
            //     .Enrich.WithProperty("Environment", environment)
            //     .WriteTo.Console(outputTemplate: "[{Timestamp:yyy-MM-dd HH:mm:ss.fff zzz} {Level}] {Message} ({SourceContext:l}){NewLine}{Exception}");
            //
            //
            // loggerConfiguration = loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            // {
            //     AutoRegisterTemplate = true,
            //     IndexFormat = $"leatherback-PaymentGateway-api-{DateTime.UtcNow:yyyy-MM}",
            //     CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
            //     MinimumLogEventLevel = LogEventLevel.Verbose,
            //     AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
            // });
            // Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}