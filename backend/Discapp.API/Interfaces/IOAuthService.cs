using Discapp.API.Models;

namespace Discapp.API.Interfaces
{
public interface IOAuthService
    {
        Task<OAuthTokenResponse> GetRequestTokenAsync();
        Task<OAuthTokenResponse> GetAccessTokenAsync(string oauthToken, string oauthVerifier, string tokenSecret);
    }
}
