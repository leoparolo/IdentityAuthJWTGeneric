using IdentityAuthModule.DTO;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthModule.CredentialValidator
{
    public interface ICredentialValidator
    {
        Task<IdentityUser> ValidateAsync(LoginRequest _loginRequest);
    }
}
