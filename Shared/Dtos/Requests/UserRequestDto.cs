using DietiEstate.Shared.Enums;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.Shared.Dtos.Requests;

/// <summary>
/// Represents a Data Transfer Object for creating or updating a <see cref="User"/> entity.
/// </summary>
public class UserRequestDto
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <remarks>
    /// The password should be hashed before saving it to the database.
    /// </remarks>
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="UserRole.Client"/>>
    /// </remarks>
    public UserRole Role { get; init; } = UserRole.Client;
}