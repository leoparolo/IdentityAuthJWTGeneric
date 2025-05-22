using IdentityAuthModule.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAuthModule.Infrastructure.Services
{
    public class ClaimsGenerate(UserManager<IdentityUser> userManager) : IClaimsGenerate
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<List<Claim>> CreateClaimsAsync(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            return userClaims.ToList()!;
        }
    }
}
