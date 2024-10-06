using Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.ResponseModel.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.Authorization.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly StrJWT _strJWT;

        public JwtUtils(IOptions<StrJWT> strJWT)
        {
            _strJWT = strJWT.Value;
        }

        public string GenerateToken(Guid userId, string userName, string email)
        {
            string? skey = _strJWT.Key;
            string? issuer = _strJWT.Issuer;
            string? audience = _strJWT.Audience;
            // generate token that is valid for 7 days
            var key = Encoding.ASCII.GetBytes(skey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, userName),
                    new Claim("Id", userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_strJWT.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                Guid userId = Guid.Empty;
                Guid.TryParse(jwtToken.Claims.First(x => x.Type == "Id").Value, out userId);

                if (userId == Guid.Empty) return null;
                // return user id from JWT token if validation successful
                return userId;
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return null;
            }
        }

        public RfTokenResponse GenerateRefreshToken(Guid userId, string userName, string email, string skey, string Issuer, string Audience, string ipAddress)
        {
            var key = Encoding.ASCII.GetBytes(skey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, userName),
                    new Claim("Id", userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Email, email ),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return new RfTokenResponse
            {
                Token = jwtToken,
                Expires = DateTime.UtcNow.AddDays(1),
                CreateTime = DateTime.Now,
                CreatedByIp = ipAddress
            };
        }

        /*public RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    CreateTime = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        public RefreshToken GenerateRefreshToken(Guid userId, string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(60),
                    CreateTime = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    UserId = userId
                };
            }
        }*/
    }
}
