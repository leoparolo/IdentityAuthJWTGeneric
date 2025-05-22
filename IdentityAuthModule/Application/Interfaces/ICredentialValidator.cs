using IdentityAuthModule.Application.DTO.Requests;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthModule.Application.Interfaces
{
    public interface ICredentialValidator
    {
        Task<IdentityUser> ValidateAsync(LoginRequest _loginRequest);
    }
}
