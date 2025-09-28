using DietiEstate.Shared.Enums;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.Shared.Dtos.Filters;

/// <summary>
/// Data transfer object containing filter parameters for <see cref="User"/> queries.
/// Used to specify criteria for filtering, sorting, and retrieving users.
/// </summary>
public class UserFilterDto
{
    /// <summary>
    /// Gets or sets the property type identifier to filter users by a specific first name.
    /// If null, no first name filtering is applied.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the property type identifier to filter users by a specific last name.
    /// If null, no last name filtering is applied.
    /// </summary>
    public string LastName { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the property type identifier to filter users by a specific email.
    /// If null, no email filtering is applied.
    /// </summary>
    public string Email { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user role to filter users by a specific role.
    /// If null, no role filtering is applied.
    /// </summary>
    public UserRole? Role { get; init; }
    
    /// <summary>
    /// Gets or sets the field name to sort the results by.
    /// Supported values include: "firstName", "lastName", "email", "role".
    /// The default value is "firstName".
    /// </summary>
    public string SortBy { get; init; } = "firstName";
    
    /// <summary>
    /// Gets or sets the sort order direction.
    /// Valid values are "asc" for ascending or "desc" for descending order.
    /// The default value is "desc".
    /// </summary>
    public string SortOrder { get; init; } = "desc";
}