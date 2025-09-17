using System.Security.Claims;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Configs;
using Microsoft.AspNetCore.Authorization;

namespace DietiEstate.WebApi.Handlers;

/// <summary>
/// A custom authorization handler that validates a user's role against a minimum role requirement.
/// </summary>
/// <remarks>
/// This handler is responsible for enforcing role-based access control by comparing the
/// user's role claim to the minimum role specified in the <see cref="MinimumRoleRequirement"/>.
/// If the user's role is equal to or higher than the required minimum role, the requirement is satisfied.
/// Roles are expected to be defined hierarchically using the <see cref="UserRole"/> enum.
/// </remarks>
/// <example>
/// This handler is typically registered as a service in the application's dependency
/// injection container and used in conjunction with authorization policies.
/// Refer to the application's configuration in Program.cs to see the usage of this handler.
/// </example>
public class MinimumRoleHandler : AuthorizationHandler<MinimumRoleRequirement>
{
    /// Handles the authorization requirement by evaluating the user's role against the defined minimum role.
    /// <param name="context">
    /// The authorization handler context, which provides information about the user and resource being authorized.
    /// </param>
    /// <param name="requirement">
    /// The requirement that specifies the minimum role needed for authorization.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. If the user's role satisfies the requirement,
    /// the context is updated to indicate success. Otherwise, no action is taken.
    /// </returns>
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