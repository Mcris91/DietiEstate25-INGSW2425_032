using System.Security.Claims;

namespace DietiEstate.WebApi.Services.Interfaces;

public interface IUserScopeService
{
    bool HasScope(ClaimsPrincipal user, string action, string resource);
    bool CanPerformAction(ClaimsPrincipal user, string action, string resource);
}