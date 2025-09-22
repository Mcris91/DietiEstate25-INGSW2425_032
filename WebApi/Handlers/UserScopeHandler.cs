using DietiEstate.WebApi.Configs;
using Microsoft.AspNetCore.Authorization;

namespace DietiEstate.WebApi.Handlers;

public class UserScopeHandler : AuthorizationHandler<UserScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserScopeRequirement requirement)
    {
        var userScopes = context.User.FindAll("scope")
            .Select(c => c.Value)
            .ToList();

        if (userScopes.Contains(requirement.FullScope))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}