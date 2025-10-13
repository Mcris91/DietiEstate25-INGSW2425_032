using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DietiEstate.Infrastracture.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return userId is not null
            ? Guid.Parse(userId)
            : Guid.Empty;
    }
}