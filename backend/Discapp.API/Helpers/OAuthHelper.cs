using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Discapp.API.Helpers
{
	public static class OAuthHelper
	{
		public static SortedDictionary<string, string> GenerateOAuthParameters(string consumerKey, string token, string callbackUrl)
		{
			SortedDictionary<string, string> parameters = new()
			{
				{ "oauth_consumer_key", consumerKey },
				{ "oauth_token", token },
				{ "oauth_signature_method", "HMAC-SHA1" },
				{ "oauth_timestamp", GenerateTimeStamp() },
				{ "oauth_nonce", GenerateNonce() },
				{ "oauth_version", "1.0" }
			};

			if (!string.IsNullOrEmpty(callbackUrl))
			{
				parameters.Add("oauth_callback", callbackUrl);
			}

			return parameters;
		}

		public static string GenerateTimeStamp()
		{
			long spongebob = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			return spongebob.ToString();
		}

		public static string GenerateNonce()
		{
			Random random = new();
			string nonce = random.Next(123400, 9999999).ToString();
			return nonce;
		}

		public static string CreateSignatureBaseString(string method, string baseUrl, SortedDictionary<string, string> parameters)
		{
			string paramString = CreateParameterString(parameters);
			return $"{method.ToUpper()}&{Uri.EscapeDataString(baseUrl)}&{Uri.EscapeDataString(paramString)}";
		}

		private static string CreateParameterString(SortedDictionary<string, string> parameters)
		{
			List<string> paramList = new();
			foreach (var param in parameters)
			{
				paramList.Add($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}");
			}
			return string.Join("&", paramList);
		}

		public static string GenerateSignature(string signatureBase, string consumerSecret, string tokenSecret)
		{
			string signingKey = $"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";
			using HMACSHA1 hasher = new(Encoding.ASCII.GetBytes(signingKey));
			byte[] hashBytes = hasher.ComputeHash(Encoding.ASCII.GetBytes(signatureBase));
			return Convert.ToBase64String(hashBytes);
		}

		public static string BuildAuthorizationHeader(SortedDictionary<string, string> oauthParameters, string signature)
		{
			oauthParameters.Add("oauth_signature", Uri.EscapeDataString(signature));
			string authHeader = "OAuth ";
			foreach (var param in oauthParameters)
			{
				authHeader += $"{Uri.EscapeDataString(param.Key)}=\"{Uri.EscapeDataString(param.Value)}\", ";
			}
			return authHeader.TrimEnd(',', ' ');
		}

		public static Dictionary<string, string> ParseQueryString(string queryString)
		{
			Dictionary<string, string> queryParams = new();
			string[] pairs = queryString.Split('&');
			foreach (var pair in pairs)
			{
				string[] keyValue = pair.Split('=');
				queryParams.Add(keyValue[0], keyValue[1]);
			}
			return queryParams;
		}
	}
}
