using System.Security.Claims;
using DietiEstate.Shared.Models.UserModels;
using Microsoft.AspNetCore.Authorization;

namespace DietiEstate.WebApi.Configs;

public class MinimumRoleHandler : AuthorizationHandler<MinimumRoleRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumRoleRequirement requirement)
    {
        var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (roleClaim == null || !Enum.TryParse<UserRole>(roleClaim, out var userRole)) 
            return Task.CompletedTask;
        if (userRole >= requirement.MinimumRole)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }    
}