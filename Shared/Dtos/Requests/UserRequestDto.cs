using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.Shared.Dtos.Requests;

/// <summary>
/// Represents a Data Transfer Object (DTO) for handling user-related requests.
/// </summary>
/// <remarks>
/// This DTO is used for creating or updating user entities across the application.
/// It contains properties that define the essential attributes of a user.
/// </remarks>
public class UserRequestDto
{
    /// <summary>
    /// Gets or initializes the email address of the user.
    /// </summary>
    /// <remarks>
    /// This property is used to represent the user's email address and is expected
    /// to be unique across all users. It is validated for duplicates during user creation.
    /// </remarks>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Represents the password for a user in the system.
    /// </summary>
    /// <remarks>
    /// This property is used for creating new users or authenticating existing users.
    /// The password should meet the required validation criteria and is securely
    /// hashed before storage.
    /// It is expected to be provided during user registration and processed
    /// securely before any further usage in the system.
    /// </remarks>
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    /// <remarks>
    /// Represents the given name of the user. Typically paired with <see cref="LastName"/> to define the full name.
    /// </remarks>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    /// <remarks>
    /// This property represents the surname or family name of the user.
    /// It is used in operations involving user management, such as creating, updating, or displaying user details.
    /// </remarks>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Represents the role of a user in the system.
    /// </summary>
    /// <remarks>
    /// The role is defined by the <see cref="UserRole"/> enumeration and determines the user's permissions
    /// and access levels within the system. Default value is <see cref="UserRole.Client"/>.
    /// </remarks>
    public UserRole Role { get; init; } = UserRole.Client;
}