using System.ComponentModel.DataAnnotations;

namespace Model.RequestModel;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}

public class SendOTPLoginRequest
{
    [Required]
    public string UserPhone { get; set; }
}

public class LoginByOTPRequest : SendOTPLoginRequest
{
    [Required]
    public string OTP { get; set; }
}

public class LoginByQrCodeRequest
{
    [Required]
    public string QrCode { get; set; }
}

public class ClearBlackListSmsRequest
{
    public string? UserPhone { get; set; }
}

public class DeviceInfoRequest
{
    public string UDID { get; set; } = null!;
    public string? OSVersion { get; set; }
    public string? OSName { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceName { get; set; }
    public string? DeviceDescription { get; set; }
}