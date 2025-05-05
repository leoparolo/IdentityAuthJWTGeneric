using IdentityAuthModule.DTO;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthModule.CredentialValidator
{
    public class CredentialValidator : ICredentialValidator
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CredentialValidator(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> ValidateAsync(LoginRequest _loginRequest)
        {
            if (string.IsNullOrWhiteSpace(_loginRequest.UserName) || string.IsNullOrWhiteSpace(_loginRequest.Password))
                throw new UnauthorizedAccessException("Ingrese su credencial.");

            var user = await _userManager.FindByNameAsync(_loginRequest.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, _loginRequest.Password))
                return user;

            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }
    }
}
