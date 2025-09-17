namespace DietiEstate.Shared.Dtos.Requests;

/// <summary>
/// Represents the data transfer object for login requests.
/// This class is used to encapsulate the necessary information required
/// for a user to authenticate by providing their email and password.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets or initializes the email address associated with the login request.
    /// This property is required for user authentication and is a unique
    /// identifier used to reference the user in the system.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets or initializes the password associated with the login request.
    /// This property is required to authenticate the user and should be
    /// provided in combination with a valid email address by the client.
    /// </summary>
    public string Password { get; init; } = string.Empty;
}