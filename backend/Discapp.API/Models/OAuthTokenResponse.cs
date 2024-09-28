namespace Discapp.API.Models
{
    public class OAuthTokenResponse
    {
        public required string OAuthToken { get; set; }
        public required string OAuthTokenSecret { get; set; }
    }
}
