using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.UserModels;

/// <summary>
/// Represents a user session in the system, containing information about authentication tokens,
/// their expiration, and session lifecycle metadata.
/// </summary>
public class UserSession
{
    /// <summary>
    /// Gets or sets the unique identifier for the user session.
    /// This is a globally unique identifier (GUID) used to distinguish and track
    /// individual user sessions in the system.
    /// </summary>
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the session.
    /// This property links the session to a specific user within the system.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the access token associated with a user session.
    /// </summary>
    /// <remarks>
    /// This property is used to authenticate and authorize user requests.
    /// The token is a string value generated during session creation and refreshed as needed.
    /// </remarks>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the refresh token associated with the user session.
    /// This token is used to obtain a new access token when the current access token has expired.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the access token expires.
    /// </summary>
    /// <remarks>
    /// This property indicates the expiration time of the user's access token.
    /// It is used to determine whether the token is still valid for authentication or
    /// needs to be refreshed.
    /// </remarks>
    public DateTime AccessTokenExpiry { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the refresh token associated with the user session.
    /// </summary>
    /// <remarks>
    /// The refresh token expiry defines the period after which the refresh token
    /// is no longer valid and cannot be used to generate new access tokens.
    /// Ensure this value is managed securely and aligns with the configured
    /// refresh token expiration policy in the system.
    /// </remarks>
    public DateTime RefreshTokenExpiry { get; set; }

    /// <summary>
    /// Specifies the date and time when the user session was created.
    /// This property is set during the initialization of a new user session
    /// and is used as a reference for session lifecycle tracking.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the last time the user session was accessed or modified.
    /// This property is updated to reflect the most recent activity associated with the session,
    /// typically during actions like refreshing tokens or validating user requests.
    /// </summary>
    public DateTime LastAccessed { get; set; }
}