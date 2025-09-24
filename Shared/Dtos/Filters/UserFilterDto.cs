using DietiEstate.Shared.Enums;

namespace DietiEstate.Shared.Dtos.Filters;

public class UserFilterDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserRole? Role { get; init; }
    
    public string SortBy { get; init; } = "firstName";
    
    /// <summary>
    /// Gets or sets the sort order direction.
    /// Valid values are "asc" for ascending or "desc" for descending order.
    /// The default value is "desc".
    /// </summary>
    public string SortOrder { get; init; } = "desc";
}