using IdentityAuthModule.DTO;

namespace IdentityAuthModule.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> ExecuteAsync(LoginRequest loginModel);
    }
}
