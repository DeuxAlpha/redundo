using System.Linq;
using System.Security.Claims;

namespace Common.Extensions
{
    public static class PrincipalExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string claimType)
        {
            return principal?.Claims.SingleOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}