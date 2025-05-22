using IdentityAuthModule.Application.DTO.Requests;
using IdentityAuthModule.Application.DTO.Responses;

namespace IdentityAuthModule.Application.Interfaces
{
    public interface ILoginUserService
    {
        Task<AuthResponse> ExecuteAsync(LoginRequest loginModel);
    }
}
