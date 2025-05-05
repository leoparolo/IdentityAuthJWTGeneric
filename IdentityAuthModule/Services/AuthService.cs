using IdentityAuthModule.ClaimsGenerate;
using IdentityAuthModule.CredentialValidator;
using IdentityAuthModule.DTO;
using IdentityAuthModule.TokenGenerate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace IdentityAuthModule.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClaimsGenerate _claimsGenerate;
        private readonly ITokenGenerate _tokenGenerate;
        private readonly ICredentialValidator _credentialValidator;

        private LoginRequest _loginRequest = new();
        private DateTime _tokenExpirationTime;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config,IClaimsGenerate claimsGenerate, ITokenGenerate tokenGenerate, ICredentialValidator credentialValidator)
        {
            _userManager = userManager;
            _claimsGenerate = claimsGenerate;
            _tokenGenerate = tokenGenerate;
            _credentialValidator = credentialValidator;
        }
        public async Task<AuthResponse> ExecuteAsync(LoginRequest loginModel)
        {
            _loginRequest = loginModel;
            _tokenExpirationTime = DateTime.Now.AddHours(12);
            return await LoginAsync();
        }

        public async Task<AuthResponse> LoginAsync()
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
