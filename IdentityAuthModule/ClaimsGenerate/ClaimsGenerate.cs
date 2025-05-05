using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAuthModule.ClaimsGenerate
{
    public class ClaimsGenerate : IClaimsGenerate
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ClaimsGenerate(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<Claim>> CreateClaimsAsync(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            return userClaims.ToList();
        }
    }
}
