using IdentityAuthModule.Application.DTO.Requests;
using IdentityAuthModule.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthModule.Infrastructure.Services
{
    public class CredentialValidator(UserManager<IdentityUser> userManager) : ICredentialValidator
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

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
