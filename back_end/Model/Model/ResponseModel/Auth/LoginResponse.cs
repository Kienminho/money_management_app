namespace Model.ResponseModel.Auth
{
    public class LoginResponse
    {
        private string? _token;
        private string? _Refreshtoken;
        public Guid userId { get; set; }
        public string? AccessToken { get { return _token; } }
        public string? RefreshToken { get { return _Refreshtoken; } }

        public void SetToken(string token)
        {
            _token = token;
        }

        public void SetRefreshToken(string RefreshToken)
        {
            _Refreshtoken = RefreshToken;
        }
    }

    public class RfTokenResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
    }
}
