using DomainService.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoneyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IHttpContextAccessor httpContextAccessor, IAuthService authService) : BaseController(httpContextAccessor)
    {
        private readonly IAuthService _authService = authService;

        [AllowAnonymous]
        [HttpGet("login-google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed.");

            var accessToken = authenticateResult.Properties.GetTokenValue("access_token");

            var result = await _authService.SignInGoogle(accessToken ?? "");
            return Ok(result);
        }
    }
}
