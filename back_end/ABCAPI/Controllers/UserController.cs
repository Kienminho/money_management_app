using Common.Authorization;
using Common.Constant;
using Common.Enum;
using Common.Utils;
using DomainService.Interfaces;
using DomainService.Interfaces.ABC;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModel;

namespace ABCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        public UserController(IHttpContextAccessor httpContextAccessor, ILoginService loginService, IUserService userService) : base(httpContextAccessor)
        {
            _loginService = loginService;
            _userService = userService;
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res = _loginService.Login(model, deviceInfo, IpAddress());
            return Ok(res, model);
        }

        /// <summary>
        /// Lấy token mới từ refresh token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("get-token-by-refresh")]
        public async Task<IActionResult> GetNewTokenByRefreshToken([FromBody] RefreshTokenRequest model)
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res = _loginService.GetNewTokenByRefreshToken(model, deviceInfo, IpAddress());
            return Ok(res, model);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest model)
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res = _loginService.RevokeToken(model, deviceInfo, IpAddress());
            return Ok(res, model);
        }

        /// <summary>
        /// Send OTP login to phone number
        /// </summary>
        /// <param name="model">UDID: Unique Device Identifier</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("send-otp-login")]
        public async Task<IActionResult> SendOTPLoginToPhone([FromBody] SendOTPLoginRequest model)
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res = _loginService.SendOTPLoginToPhone(model, deviceInfo);
            return Ok(res, model);
        }

        /// <summary>
        /// Login by otp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login-by-otp")]
        public async Task<IActionResult> LoginByOTP([FromBody] LoginByOTPRequest model)
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res = _loginService.LoginByOTP(model, deviceInfo, IpAddress());
            return Ok(res, model);
        }

        /// <summary>
        /// Get QR login
        /// </summary>
        /// <param name="UDID">Unique Device Identifier</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("qr-login")]
        public async Task<IActionResult> GetQRLogin()
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res = _loginService.GetQRLogin(deviceInfo);
            return Ok(res, deviceInfo);
        }

        /// <summary>
        /// Phone verify qr login
        /// </summary>
        /// <param name="UDID">Unique Device Identifier</param>
        /// <returns></returns>
        [HttpPost("verify-qr-login")]
        public async Task<IActionResult> VerifyQRCode([FromBody] LoginByQrCodeRequest model)
        {
            var guidUserId = Guid.Parse(userId);
            var res = _loginService.VerifyQRCode(guidUserId, model);
            return Ok(res, model);
        }

        /// <summary>
        /// Login by qrcode
        /// Note: Increase api timeout
        /// </summary>
        /// <param name="model">UDID: Unique Device Identifier</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login-by-qr")]
        public async Task<IActionResult> LoginByQrCode([FromBody] LoginByQrCodeRequest model)
        {
            var deviceInfo = Utils.GetDeviceInfo(Request);
            var res =  await _loginService.WaitVerifyQrCode(model, deviceInfo, IpAddress());
            return Ok(res, model);
        }

        /// <summary>
        /// Clear black list sms
        /// </summary>
        /// <param name="userPhone">null: clear all, have value: clear special phone</param>
        /// <returns></returns>
        [HttpPost("clear-black-list-sms")]
        public async Task<IActionResult> ClearBlackListSms([FromBody] ClearBlackListSmsRequest model)
        {
            _loginService.ClearBlackListSms(model);
            return Ok(new object(), model);
        }

        /// <summary>
        /// Lấy danh sách user
        /// </summary>
        /// <returns></returns>
        [Authorization(activityCode = PermissionConstant.ACTIVITY_USER, activity = ActivityType.Read)]
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList()
        {
            var res = _userService.GetList();
            return Ok(res, res.Count);
        }

        /// <summary>
        /// Lấy chi tiết theo userId
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var res = _userService.GetById(id);
            return Ok(res);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}