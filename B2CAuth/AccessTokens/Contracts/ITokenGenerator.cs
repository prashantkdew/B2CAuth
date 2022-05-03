namespace B2CAuth.AccessTokens.Contracts
{
    using B2CAuth.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract Interface for Token Generator
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Method to get Token string
        /// </summary>
        /// <returns></returns>
        Task<B2CTokenResponse> GetTokenAsync();
    }
}
