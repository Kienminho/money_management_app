using Common.Authorization.Utils;
using Common.Settings;
using Common.UnitOfWork.UnitOfWorkPattern;
using Common.Utils;
using DomainService.Interface;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Model.ResponseModel.Auth;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Infrastructure.Implements
{
    public class AuthService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IOptions<StrJWT> options, IJwtUtils jwtUtils) : BaseService(unitOfWork, memoryCache), IAuthService
    {
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private StrJWT _strJWT = options.Value;

        public async Task<GoogleProfileResponse> GetGoogleProfile(string accessToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var info = JsonConvert.DeserializeObject<GoogleProfileResponse>(content);
            return info ?? new GoogleProfileResponse();
        }

        public async Task<object> SignInGoogle(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Login fail");
            }

            var profile = await GetGoogleProfile(accessToken);

            if (profile == null)
                throw new Exception("Login fail");

            //check exist user
            var user = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Email == profile.email && u.IsDeleted != true);
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = profile.given_name,
                    LastName = profile.family_name,
                    Email = profile.email,
                    UserName = profile.name,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    Creator = "System",
                };

                //generate token
                var token = _jwtUtils.GenerateToken(user.Id, user.UserName, user.Email);
                var refreshToken = _jwtUtils.GenerateRefreshToken(user.Id, user.UserName, user.Email, _strJWT.Key, _strJWT.Issuer, _strJWT.Audience, null);

                user.AccesToken = token;
                user.RefreshToken = refreshToken.Token;

                _unitOfWork.Repository<User>().Add(user);
            }
            else
            {
                //generate token
                var token = _jwtUtils.GenerateToken(user.Id, user.UserName, user.Email);
                var refreshToken = _jwtUtils.GenerateRefreshToken(user.Id, user.UserName, user.Email, _strJWT.Key, _strJWT.Issuer, _strJWT.Audience, null);

                user.AccesToken = token;
                user.RefreshToken = refreshToken.Token;

                _unitOfWork.Repository<User>().Update(user);
            }

            var res = new LoginResponse
            {
                userId = user.Id,
            };

            res.SetToken(user.AccesToken);
            res.SetRefreshToken(user.RefreshToken);

            await _unitOfWork.SaveChangesAsync();

            return Utils.CreateResponseModel(res);
        }
    }
}
