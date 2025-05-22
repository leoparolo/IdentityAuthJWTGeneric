using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAuthModule.Application.Interfaces
{
    public interface ITokenGenerate
    {
        Task<string> GenerateTokenAsync(IdentityUser user, List<Claim> claims, DateTime expiration);
    }
}
