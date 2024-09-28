using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Discapp.API.Models;
using Discapp.API.Interfaces;
using Discapp.API.Helpers;

namespace Discapp.API.Services
{
    public class OAuthService(IHttpClientFactory httpClientFactory) : IOAuthService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly string _consumerKey = "aaa";
        private readonly string _consumerSecret = "aaa";
        private readonly string _callbackUrl = "your-frontend-callback-url";

		public async Task<OAuthTokenResponse> GetRequestTokenAsync()
        {
            var method = "GET";
            var url = "https://api.discogs.com/oauth/request_token";

            SortedDictionary<string, string> oauthParameters = OAuthHelper.GenerateOAuthParameters(_consumerKey, string.Empty, _callbackUrl);
            string signatureBase = OAuthHelper.CreateSignatureBaseString(method, url, oauthParameters);
            string signature = OAuthHelper.GenerateSignature(signatureBase, _consumerSecret, string.Empty);
			string authHeader = OAuthHelper.BuildAuthorizationHeader(oauthParameters, signature);

            HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Add("Authorization", authHeader);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            Dictionary<string, string> responseParams = OAuthHelper.ParseQueryString(content);
            return new OAuthTokenResponse
            {
                OAuthToken = responseParams["oauth_token"],
                OAuthTokenSecret = responseParams["oauth_token_secret"]
            };
        }

        public async Task<OAuthTokenResponse> GetAccessTokenAsync(string oauthToken, string oauthVerifier, string tokenSecret)
        {
            string method = "POST";
            string url = "https://api.discogs.com/oauth/access_token";

            SortedDictionary<string, string> oauthParameters = OAuthHelper.GenerateOAuthParameters(_consumerKey, oauthToken, null);
            oauthParameters.Add("oauth_verifier", oauthVerifier);
            string signatureBase = OAuthHelper.CreateSignatureBaseString(method, url, oauthParameters);
            string signature = OAuthHelper.GenerateSignature(signatureBase, _consumerSecret, tokenSecret);
            string authHeader = OAuthHelper.BuildAuthorizationHeader(oauthParameters, signature);

            HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Headers.Add("Authorization", authHeader);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            Dictionary<string, string> responseParams = OAuthHelper.ParseQueryString(content);
            return new OAuthTokenResponse
            {
                OAuthToken = responseParams["oauth_token"],
                OAuthTokenSecret = responseParams["oauth_token_secret"]
            };
        }
	}
}
