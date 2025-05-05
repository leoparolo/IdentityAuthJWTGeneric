using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAuthModule.ClaimsGenerate
{
    public interface IClaimsGenerate
    {
        Task<List<Claim>> CreateClaimsAsync(IdentityUser user);
    }
}
