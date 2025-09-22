using System.Security.Claims;
using DietiEstate.WebApi.Services.Interfaces;

namespace DietiEstate.WebApi.Services.Implementations;

public class UserScopeService : IUserScopeService
{
    public bool HasScope(ClaimsPrincipal user, string action, string resource)
    {
        var userScopes = GetUserScopes(user).ToList();
        var requiredScope = $"{action}:{resource}";

        return userScopes.Contains(requiredScope);
    }

    public bool CanPerformAction(ClaimsPrincipal user, string action, string resource)
    {
        return HasScope(user, action, resource);
    }

    private static IEnumerable<string> GetUserScopes(ClaimsPrincipal user)
    {
        return user.FindAll("scope").Select(c => c.Value);
    }
}