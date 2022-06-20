using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApplicationServices.Common.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace CardServiceAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private IWebHostEnvironment _environment;
        private ILogger<ErrorHandlingMiddleware> Logger { get; set; }

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            this.Logger = logger;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ApplicationServices.Common.Exceptions.ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    string[] errorArray = validationException.Failures.SelectMany(x => x.Value).ToArray();
                    var error = string.Join(";", errorArray);
                    result = JsonConvert.SerializeObject(Result.Fail(error));
                    this.Logger.LogInformation($"An error occured.......{result}");
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                case InvalidCardException validationException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    result = JsonConvert.SerializeObject(Result.Fail(string.IsNullOrEmpty(validationException.Message)
                        ? "An error occured please contact Leatherback support."
                        : validationException.Message));
                    this.Logger.LogInformation(result);
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                case EntityNotFoundException entityNotFoundException:
                    code = HttpStatusCode.NotFound;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    result = JsonConvert.SerializeObject(Result.Fail(
                        string.IsNullOrEmpty(entityNotFoundException.Message)
                            ? "Entity not found. Please refine your search criteria and try again."
                            : entityNotFoundException.Message));
                    this.Logger.LogInformation(result);
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                case ProviderNotFoundException providerNotFoundException:
                    code = HttpStatusCode.NotFound;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    result = JsonConvert.SerializeObject(Result.Fail(
                        string.IsNullOrEmpty(providerNotFoundException.Message)
                            ? "Provider not found. Please refine your search criteria and try again."
                            : providerNotFoundException.Message));
                    this.Logger.LogInformation(result);
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                case InvalidInputException invalidInputException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    result = JsonConvert.SerializeObject(Result.Fail(string.IsNullOrEmpty(invalidInputException.Message)
                        ? "An error occured on the payload"
                        : invalidInputException.Message));
                    this.Logger.LogInformation(result);
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                case DuplicateException duplicateException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    result = JsonConvert.SerializeObject(Result.Fail(string.IsNullOrEmpty(duplicateException.Message)
                        ? "An error occured on the payload"
                        : duplicateException.Message));
                    this.Logger.LogInformation(result);
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                case InvalidEntityException invalidEntityException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    result = JsonConvert.SerializeObject(Result.Fail(
                        string.IsNullOrEmpty(invalidEntityException.Message)
                            ? "An error occured on the payload"
                            : invalidEntityException.Message));
                    this.Logger.LogInformation(result);
                    this.Logger.LogInformation(
                        "==================================================================================");
                    break;
                default:

                    code = HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) code;
                    
                    result = JsonConvert.SerializeObject(Result.Fail(
                        string.IsNullOrEmpty(exception.Message)
                            ? "An error occured on the payload"
                            : exception.Message));
                    
                    this.Logger.LogInformation(exception.ToString());
                    this.Logger.LogInformation(
                        "==================================================================================");
 
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            if (string.IsNullOrEmpty(result))
            {
                if (!_environment.IsProduction())
                {
                    result = JsonConvert.SerializeObject(
                        Result.Fail("An error occured. Please contact leatherback support", exception),
                        Formatting.Indented,
                        new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                }
                else
                {
                    result = JsonConvert.SerializeObject(
                        Result.Fail("An error occured. Please contact leatherback support"));
                }

                this.Logger.LogInformation(
                    $"An error occured.......{JsonConvert.SerializeObject(exception, Formatting.Indented, new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore})}");
                this.Logger.LogInformation(
                    "==================================================================================");
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}