using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenManagement; 
using IdentityModel.Client;
using Microsoft.Extensions.Configuration; 

namespace CardServiceAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenExchangeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomingToken"></param>
        /// <param name="apiScope"></param>
        /// <returns></returns>
        Task<string> GetTokenAsync(string incomingToken, string apiScope);
    }
    /// <summary>
    /// 
    /// </summary>
    public class TokenExchangeService : ITokenExchangeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IClientAccessTokenCache _tokenCache;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="tokenCache"></param>
        /// <param name="configuration"></param>
        public TokenExchangeService(IHttpClientFactory httpClientFactory,
            IClientAccessTokenCache tokenCache, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _tokenCache = tokenCache;
            _configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomingToken"></param>
        /// <param name="apiScope"></param>
        /// <returns></returns>
        //return a non-expired access token
        public async Task<string> GetTokenAsync(string incomingToken, string apiScope)
        {
            //get token from the cache
            //appending the OnboardingService audience to the client name to prevent overriding tokens
            //this will get access token for the OnboardingService
            var item = await _tokenCache.GetAsync($"onboardingtodownstreamtokenexchangeclient_{apiScope}");
            if (item != null)
            {
                return item.AccessToken;
            }

            var client = _httpClientFactory.CreateClient();

            var discoveryDocumentResponse = await
                client.GetDiscoveryDocumentAsync(_configuration["IdentityServiceConfiguration"]);

            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var customParameters = new Dictionary<string, string>
            {
                { "subject_token_type", "urn:ietf:params:oauth:grant-type:token-exchange" },
                { "subject_token", incomingToken },
                { "scope", $"openid profile {apiScope}" }
            };

            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = customParameters,
                ClientId = "onboardingtodownstreamtokenexchangeclient",
                ClientSecret = "secret"

            });

            if (response.IsError)
            {
                throw new Exception(response.ErrorDescription);
            }

            await _tokenCache.SetAsync($"onboardingtodownstreamtokenexchangeclient_{apiScope}",
                response.AccessToken, response.ExpiresIn);

            return response.AccessToken;
        }


    }
}
