using Microsoft.AspNetCore.Authorization;

namespace DietiEstate.WebApi.Configs;

public class UserScopeRequirement(string action, string resource) : IAuthorizationRequirement
{
    private string RequiredAction { get; } = action;
    private string RequiredResource { get; } = resource;
    public string FullScope => $"{RequiredAction}:{RequiredResource}";
}