using DietiEstate.Shared.Models.UserModels;
using Microsoft.AspNetCore.Authorization;

namespace DietiEstate.WebApi.Configs;

/// <summary>
/// Represents a custom authorization requirement that specifies the minimum
/// user role needed to access a resource or perform an action.
/// </summary>
/// <remarks>
/// This requirement is used in conjunction with the <see cref="MinimumRoleHandler"/>
/// to enforce role-based access control policies. The user must have a role
/// equal to or higher than the specified minimum role to satisfy the requirement.
/// The roles are defined in the <see cref="UserRole"/> enum and ordered
/// hierarchically, where higher roles have more privileges.
/// </remarks>
public class MinimumRoleRequirement(UserRole minimumRole) : IAuthorizationRequirement
{
    public UserRole MinimumRole { get; } = minimumRole;
}