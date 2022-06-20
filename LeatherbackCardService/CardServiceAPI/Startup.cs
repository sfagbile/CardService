using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using ApplicationServices;
using CardServiceAPI.Middleware;
using CardServiceAPI.Services;
using FluentValidation.AspNetCore;
using Infrastructure;
using LeatherBack.SharedLibrary.Logger.Extenstions;
using MessageBus.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Amqp.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Persistence;
using Shared;
using Shared.BaseResponse;
using Shared.ErrorModel;

namespace CardServiceAPI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();

            services.AddHttpClient<ITokenExchangeService, TokenExchangeService>();
            services.AddHttpClient<ITokenValidationService, TokenValidationService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CardServiceCorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            services.AddInfrastructure(Configuration);
            services.AddSharedDependencyInjection();
            services.AddPersistence(Configuration);
            services.AddMessagingBus(WebHostEnvironment, Configuration);
            services.AddApplication(Configuration);
            services.AddControllers();
            services.AddSingleton<IDictionary<string, string>>(opts => new Dictionary<string, string>());

            services.AddApplicationInsight(Configuration);
            // services.AddControllers().AddNewtonsoftJson(options =>
            // {
            //     options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            //     {
            //         NamingStrategy = new SnakeCaseNamingStrategy()
            //     };
            // });
            //comment to disable monitor identity server token authorization pipeline
            IdentityModelEventSource.ShowPII = true;

            //bearer token authentication scheme
            ConfigureJwt(services);

            services.AddControllers(options =>
            {
                /* options.InputFormatters.Insert(options.InputFormatters.Count,
                     new TextPlainInputFormatter()); */

                if (!WebHostEnvironment.IsDevelopment())
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorsInModelState = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(pair => pair.Key,
                            pair => pair.Value.Errors.Select(x => x.ErrorMessage))
                        .ToArray();
                    var errorResponse = new ErrorResponse();
                    //cycle through the errors and add to response
                    foreach (var (key, value) in errorsInModelState)
                    {
                        foreach (var subError in value)
                        {
                            errorResponse.Errors.Add(new Error
                            {
                                Code = key,
                                Message = subError
                            });
                        }
                    }

                    var error = Result.Fail(
                        errorResponse,
                        "Error occured",
                        StatusCodes.Status400BadRequest.ToString());

                    return new BadRequestObjectResult(error);
                };
            }).AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.Load("ApplicationServices")));

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Card Service", Version = "v1" });
                c.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
            });
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("secrets.json", optional: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
                app.UseDeveloperExceptionPage();
            }
            app.AddLeatherbackLoggerMiddleWare();
            app.UseCustomExceptionHandler();
            app.UseRouting();

            //Authetication middleware layer
            app.UseAuthentication();

            app.UseCors("CardServiceCorsPolicy");
            app.UseFileServer();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (!env.IsDevelopment())
                {
                }
                c.SwaggerEndpoint("v1/swagger.json", "CardService");
            });

            app.UsePathBase("/CardService");

            //Authorization middleware layer
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }

}