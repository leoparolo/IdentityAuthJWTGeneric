using IdentityAuthModule.Application.DTO.Requests;
using IdentityAuthModule.Application.DTO.Responses;
using IdentityAuthModule.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAuthModule.Application.UseCases.Auth.Commands
{
    public class LoginUserService(IClaimsGenerate claimsGenerate, ITokenGenerate tokenGenerate, ICredentialValidator credentialValidator) : ILoginUserService
    {
        private readonly IClaimsGenerate _claimsGenerate = claimsGenerate;
        private readonly ITokenGenerate _tokenGenerate = tokenGenerate;
        private readonly ICredentialValidator _credentialValidator = credentialValidator;

        private LoginRequest _loginRequest = new();
        private DateTime _tokenExpirationTime;

        public async Task<AuthResponse> ExecuteAsync(LoginRequest loginModel)
        {
            _loginRequest = loginModel;
            _tokenExpirationTime = DateTime.Now.AddHours(12);
            return await LoginAsync();
        }

        private async Task<AuthResponse> LoginAsync()
        {
            IdentityUser user = await _credentialValidator.ValidateAsync(_loginRequest);

            List<Claim> claims = await _claimsGenerate.CreateClaimsAsync(user);
            string token = await _tokenGenerate.GenerateTokenAsync(user, claims, _tokenExpirationTime);

            return new AuthResponse
            {
                Token = token,
                Expiration = _tokenExpirationTime
            };
        }

    }
}
