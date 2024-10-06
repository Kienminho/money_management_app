using Model.ResponseModel.Auth;

namespace DomainService.Interface
{
    public interface IAuthService
    {
        Task<object> SignInGoogle(string accessToken);
        Task<GoogleProfileResponse> GetGoogleProfile(string accessToken);
    }
}
