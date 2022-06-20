using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using IdentityModel.Client;

namespace CardServiceAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenValidationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenToValidate"></param>
        /// <returns></returns>
        Task<bool> ValidateTokenAsync(string tokenToValidate);
    }
    /// <summary>
    /// 
    /// </summary>
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public TokenValidationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenToValidate"></param>
        /// <returns></returns>
        public async Task<bool> ValidateTokenAsync(string tokenToValidate)
        {
            var client = _httpClientFactory.GetHttpClient();

            var discoveryDocumentResponse = await
                client.GetDiscoveryDocumentAsync(_configuration["IdentityServiceConfiguration"]);

            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            try
            {
                //signing key
                //create a new RSAkey from the jsonwebkey 
                var issuerSigningKeys = new List<SecurityKey>();
                foreach (var jsonWebKey in discoveryDocumentResponse.KeySet.Keys)
                {
                    var exponent = Base64Url.Decode(jsonWebKey.E);
                    var modulus = Base64Url.Decode(jsonWebKey.N);

                    var key = new RsaSecurityKey(new RSAParameters
                    {
                        Exponent = exponent,
                        Modulus = modulus
                    })
                    {
                        KeyId = jsonWebKey.Kid
                    };

                    issuerSigningKeys.Add(key);
                }

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "email",
                    ValidIssuer = _configuration["IdentityServiceConfiguration"],
                    IssuerSigningKeys = issuerSigningKeys
                };

                //used to validate a tokem
                //discard the returned value of rawValdiatedToken using _
                _ = new JwtSecurityTokenHandler().ValidateToken(tokenToValidate, tokenValidationParameters,
                    out var rawValdiatedToken);

                return true;
            }
            catch (SecurityTokenException)
            {
                //add logging
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
