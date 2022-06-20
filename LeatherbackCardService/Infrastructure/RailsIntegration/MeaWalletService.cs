using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.BaseResponse;
using Shared.Encryption;

namespace Infrastructure.RailsIntegration
{
    public class MeaWalletService : IMeaWalletService
    {
        private readonly HttpClient _client;
        private readonly DefaultContractResolver _contractResolver;
        private readonly IConfiguration _configuration;
        private readonly AESEncryption _encryptionUtil;

        public MeaWalletService(HttpClient client, IConfiguration configuration, AESEncryption encryptionUtil)
        {
            _contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            _client = client;
            _configuration = configuration;
            _encryptionUtil = encryptionUtil;
        }

        public async Task<Result<(TResult, TError, bool)>> Post<TResult, TError, T>(T model, string url,
            string meaTraceId, string iv) where T : class
        {
            var result = default(TResult);
            var errorResult = default(TError);

            var serializedModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = _contractResolver,
                Formatting = Formatting.Indented
            });
            var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("Mea-Secret", GenerateMeaSecret(meaTraceId, iv));
            _client.DefaultRequestHeaders.Add("Mea-Trace-Id", meaTraceId);

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

        public async Task<Result<(TResult, TError, bool)>> Post<TResult, TError, T>(T model, string url) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<(string, TError, bool)> Post<TError>(Dictionary<string, string> param, string url)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<(TResult, TError, bool)>> Get<TResult, TError>(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<(TResult, TError, bool)>> Get<TResult, TError>(string url, string iv)
        {
            var result = default(TResult);
            var errorResult = default(TError);
            var meaTraceId = Guid.NewGuid().ToString().ToLower();

            _client.DefaultRequestHeaders.Add("Mea-Secret", GenerateMeaSecret(meaTraceId, iv));
            _client.DefaultRequestHeaders.Add("Mea-Trace-Id", meaTraceId);
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


        public string GenerateMeaSecret(string meaTraceId, string iv)
        {
            var meaId = _configuration["MeaWallet:MeaId"].ToLower();
            var meakey = _configuration["MeaWallet:MeaApiKey"];
            //var iv = "00000000000000000000000000000000";

            var meaSecret = _encryptionUtil.EncryptText($"{meaTraceId}#{meaId}", meakey, iv);

            return meaSecret;
        }
    }
}