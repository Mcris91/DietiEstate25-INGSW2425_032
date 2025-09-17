namespace DietiEstate.Shared.Models.AuthModels;

/// <summary>
/// Specifies the type of JWT token being utilized in the authentication process.
/// </summary>
/// <remarks>
/// This enumeration represents the two possible types of tokens that can be generated and used:
/// - Access: Represents a short-lived token used for granting access to resources.
/// - Refresh: Represents a longer-lived token used to refresh expired access tokens without requiring re-auth
/// </remarks>
public enum JwtTokenType
{
    /// <summary>
    /// Access token used for authenticating API requests. 
    /// These tokens have a short lifespan (typically 15-60 minutes) and contain user claims and permissions.
    /// </summary>
    Access,
    
    /// <summary>
    /// Refresh token used to obtain new access tokens when they expire. 
    /// These tokens have a longer lifespan (typically days or weeks) and are stored securely to maintain user sessions.
    /// </summary>
    Refresh
}