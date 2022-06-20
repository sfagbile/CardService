using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.BaseResponse;

namespace Infrastructure.RailsIntegration
{
    public class RailsBankService : IRailsBankService
    {
        private readonly HttpClient _client;
        private readonly DefaultContractResolver _contractResolver;

        public RailsBankService(HttpClient client)
        {
            _contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            _client = client;
        }

        public async Task<Result<(TResult, TError, bool)>> Post<TResult, TError, T>(T model, string url) where T : class
        {
            var result = default(TResult);
            var errorResult = default(TError);

            var serializedModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = _contractResolver,
                Formatting = Formatting.Indented
            });
            var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var paymentResponse = JsonConvert.DeserializeObject<TResult>(jsonResponse);
                if (response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created)
                {
                    return Result.Ok<(TResult, TError, bool)>((paymentResponse, errorResult, true), "Successful");
                }
            }

            var paymentErrorResponse = JsonConvert.DeserializeObject<TError>(jsonResponse);
            return Result.Fail<(TResult, TError, bool)>("Operation did not succeed",
                (result, paymentErrorResponse, false));
        }

        public Task<(string, TError, bool)> Post<TError>(Dictionary<string, string> param, string url)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<(TResult, TError, bool)>> Get<TResult, TError>(string url)
        {
            var result = default(TResult);
            var errorResult = default(TError);

            var response = await _client.GetAsync(url);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var paymentResponse = JsonConvert.DeserializeObject<TResult>(jsonResponse);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    return Result.Ok<(TResult, TError, bool)>((paymentResponse, errorResult, true), "Successful");
                }
            }

            var paymentErrorResponse = JsonConvert.DeserializeObject<TError>(jsonResponse);
            return Result.Fail<(TResult, TError, bool)>("Operation did not succeed",
                (result, paymentErrorResponse, false));
        }

        public Task<Result<(TResult, TError, bool)>> Post<TResult, TError>(Dictionary<string, string> param, string url)
        {
            throw new System.NotImplementedException();
        }
    }
}