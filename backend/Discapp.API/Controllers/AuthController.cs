using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Discapp.API.Interfaces;
using Discapp.API.Models;

namespace Discapp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OAuthController(IOAuthService oauthService) : ControllerBase
	{
		private readonly IOAuthService _oauthService = oauthService;

		[HttpGet("RequestToken")]
		public async Task<IActionResult> GetRequestToken()
		{
			OAuthTokenResponse tokenResponse = await _oauthService.GetRequestTokenAsync();
			return Ok(tokenResponse);
		}

		[HttpGet("AccessToken")]
		public async Task<IActionResult> GetAccessToken(string oauthToken, string oauthVerifier, string oauthTokenSecret)
		{
			OAuthTokenResponse tokenResponse = await _oauthService.GetAccessTokenAsync(oauthToken, oauthVerifier, oauthTokenSecret);
			return Ok(tokenResponse);
		}
	}
}
