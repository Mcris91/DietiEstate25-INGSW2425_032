using DietiEstate.Core.Enums;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Application.Dtos.Filters;

public class UserFilterDto : BaseFilterDto
{ 
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public UserRole? Role { get; init; }

    public string SortBy { get; init; } = "firstName";

    public string SortOrder { get; init; } = "desc";
}