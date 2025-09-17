namespace DietiEstate.Shared.Dtos.Responses;

/// <summary>
/// Represents the response data structure returned upon successful user login.
/// This class includes the access token and refresh token required for
/// authenticated interaction with the application.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Gets the access token generated for the authenticated user.
    /// This token is used to authorize the user when accessing protected resources or APIs.
    /// </summary>
    public string Access { get; init; } = string.Empty;

    /// <summary>
    /// Represents the refresh token issued during the authentication process.
    /// This property is used to obtain a new access token when the current access token expires.
    /// It ensures continued authentication without requiring the user to re-enter credentials.
    /// </summary>
    public string Refresh { get; init; } = string.Empty;
}