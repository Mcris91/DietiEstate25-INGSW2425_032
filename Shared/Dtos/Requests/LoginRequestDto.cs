namespace DietiEstate.Shared.Dtos.Requests;

/// <summary>
/// Represents the data transfer object for login requests.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets or sets the email of the property.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the property.
    /// </summary>
    /// <remarks>
    /// The password should be hashed before saving it to the database.
    /// </remarks>
    public string Password { get; init; } = string.Empty;
}