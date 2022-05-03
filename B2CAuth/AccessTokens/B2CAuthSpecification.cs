using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAuth.AccessTokens
{
    using B2CAuth.AccessTokens.Contracts;
    using B2CAuth.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// B2CAuthSpecification
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class B2CAuthSpecification
    {
        /// <summary>
        /// Call this method at startup class as services.AddB2CTokenGenerator();
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddB2CTokenGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            // Set up configuration files.
            services.AddOptions();
            services.Configure<AzureB2CSettings>(configuration.GetSection("AzureB2CSettings"));

            var serviceProvider = services.BuildServiceProvider();
            var azureB2CSettings = serviceProvider.GetRequiredService<IOptions<AzureB2CSettings>>().Value;
            var logger = serviceProvider.GetService<ILogger<TokenGenerator>>();
            services.AddSingleton(typeof(ILogger), logger);
            services.AddHttpClient("AzureB2CClient", c =>
            {
                c.BaseAddress = new Uri(azureB2CSettings.TokenUrl);
            });
            services.AddScoped<ITokenGenerator, TokenGenerator>();
        }
    }
}
