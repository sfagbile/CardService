using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public abstract class PaymentBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        public readonly HttpClient client;
        public PaymentBase(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            client = httpClientFactory.CreateClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public abstract Task<TIn> PayOut<TIn, TOut>(TIn model);
        public abstract Task<TIn> Fund<TIn, TOut>(TIn model);
    }
}