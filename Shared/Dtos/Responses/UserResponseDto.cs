using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.Shared.Dtos.Responses;

/// <summary>
/// Represents the response data transfer object (DTO) for the User entity.
/// </summary>
/// <remarks>
/// This DTO is used to transfer user data from the server to the client.
/// </remarks>
public class UserResponseDto
{
    /// <summary>
    /// Represents the unique identifier for the user.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the email address associated with the user.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets the role assigned to the user, defining their level of access and permissions within the system.
    /// </summary>
    public UserRole Role { get; init; } = UserRole.Client;
}