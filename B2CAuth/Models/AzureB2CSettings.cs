namespace B2CAuth.Models
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// AzureB2CSettings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AzureB2CSettings
    {
        /// <summary>
        /// token_url
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// grant_type
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// client_id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// response_type
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// scope
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Sign in User Id
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// Login Password
        /// </summary>
        public string Password { get; set; }
    }
}
