using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAuth.AccessTokens
{
    using B2CAuth.AccessTokens.Contracts;
    using B2CAuth.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    /// <summary>
    /// Generates B2C Access token
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TokenGenerator : ITokenGenerator
    {
        /// <summary>
        /// Azure kevel keys requried for token generation
        /// </summary>
        private readonly AzureB2CSettings _azureB2CSettings;
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// Http Client factory to create azure b2c client
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        public TokenGenerator(IOptions<AzureB2CSettings> options,
            ILogger logger,
            IHttpClientFactory httpClientFactory)
        {
            _azureB2CSettings = options.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Gets token string
        /// </summary>
        /// <returns></returns>
        public async Task<B2CTokenResponse> GetTokenAsync()
        {
            _logger.LogDebug("GetTokenAsync:: Getting token from B2C.", typeof(TokenGenerator));
            try
            {
                string cacheKey = string.Concat("azureB2CToken-", _azureB2CSettings.LoginId);
                if (MemoryCache.Default.Contains(cacheKey))
                {
                    _logger.LogDebug("GetTokenAsync:: Returning token from Memory cache.", typeof(TokenGenerator));
                    return (B2CTokenResponse)MemoryCache.Default.Get(cacheKey);
                }
                else
                {
                    _logger.LogDebug("GetTokenAsync:: Calling Azure B2C token endpoint to fetch token.", typeof(TokenGenerator));
                    var client = _httpClientFactory.CreateClient("AzureB2CClient");
                    if (client.BaseAddress == null || string.IsNullOrEmpty(client.BaseAddress.AbsoluteUri))
                    {
                        client.BaseAddress = new Uri(_azureB2CSettings.TokenUrl);
                    }
                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("username", _azureB2CSettings.LoginId),
                    new KeyValuePair<string, string>("password", _azureB2CSettings.Password),
                    new KeyValuePair<string, string>("grant_type", _azureB2CSettings.GrantType),
                    new KeyValuePair<string, string>("client_id", _azureB2CSettings.ClientId),
                    new KeyValuePair<string, string>("response_type", _azureB2CSettings.ResponseType),
                    new KeyValuePair<string, string>("scope", _azureB2CSettings.Scope)
                });
                    var result = await client.PostAsync("", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<B2CTokenResponse>(resultContent);
                    MemoryCache.Default.Add(cacheKey, response, new CacheItemPolicy
                    {
                        AbsoluteExpiration =
                                new DateTimeOffset(
                                    DateTime.UtcNow.AddSeconds(response.ExpiresIn.GetValueOrDefault() - 10))
                    });
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while fetching token.");
                throw;
            }
        }

    }
}
