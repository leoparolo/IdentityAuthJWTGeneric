using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAuthModule.Application.Interfaces
{
    public interface IClaimsGenerate
    {
        Task<List<Claim>> CreateClaimsAsync(IdentityUser user);
    }
}
