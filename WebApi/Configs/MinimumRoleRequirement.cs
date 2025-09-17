using DietiEstate.Shared.Models.UserModels;
using Microsoft.AspNetCore.Authorization;

namespace DietiEstate.WebApi.Configs;

public class MinimumRoleRequirement(UserRole minimumRole) : IAuthorizationRequirement
{
    public UserRole MinimumRole { get; } = minimumRole;
}