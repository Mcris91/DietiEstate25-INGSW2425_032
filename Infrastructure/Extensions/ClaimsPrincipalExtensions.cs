using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DietiEstate.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return userId is not null
            ? Guid.Parse(userId)
            : Guid.Empty;
    }
    
    public static Guid GetAgencyId(this ClaimsPrincipal principal)
    {
        var agencyId = principal.FindFirst("AgencyId")?.Value;
        return !string.IsNullOrEmpty(agencyId)
            ? Guid.Parse(agencyId)
            : Guid.Empty;
    }
    
    public static string GetRole(this ClaimsPrincipal principal)
    {
        var role = principal.FindFirst("role")?.Value;
        return role ?? string.Empty;
    }
}