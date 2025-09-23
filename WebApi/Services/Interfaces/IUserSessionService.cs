using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Services.Interfaces;

/// <summary>
/// Defines the contract for managing user sessions, including session creation,
/// retrieval, validation, and invalidation operations.
/// </summary>
public interface IUserSessionService
{
    /// <summary>
    /// Creates a new user session and associates it with the provided user, access token,
    /// and refresh token. The session ID is returned as a string.
    /// </summary>
    /// <param name="user">The user for whom the session is being created.</param>
    /// <param name="accessToken">The access token associated with the session.</param>
    /// <param name="refreshToken">The refresh token associated with the session.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains
    /// the unique session ID as a string.</returns>
    Task<string> CreateSessionAsync(User user, string accessToken, string refreshToken);

    /// <summary>
    /// Retrieves a user session by its unique identifier.
    /// </summary>
    /// <param name="sessionId">The identifier of the session to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the
    /// <see cref="UserSession"/> if found, or null if no session exists with the specified identifier.
    /// </returns>
    Task<UserSession?> GetSessionAsync(Guid sessionId);

    /// <summary>
    /// Refreshes the access and refresh tokens for an existing user session if valid.
    /// Ensures the session is still active and the refresh token has not expired.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the session to refresh tokens for.</param>
    /// <returns>
    /// A boolean value indicating whether the session tokens were successfully refreshed.
    /// Returns false if the session does not exist, the refresh token has expired,
    /// or a failure occurs during the token refresh process.
    /// </returns>
    Task<bool> RefreshSessionTokensAsync(Guid sessionId);

    /// <summary>
    /// Invalidates a user session by removing it from the data store. This
    /// operation ensures that the session cannot be reused for authentication
    /// or authorization purposes.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the session to be invalidated.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InvalidateSessionAsync(Guid sessionId);

    /// <summary>
    /// Invalidates all active sessions for the specified user. This operation will effectively log the user out
    /// from all sessions by invalidating their associated session tokens.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose sessions are to be invalidated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InvalidateAllUserSessionsAsync(Guid userId);

    /// <summary>
    /// Checks whether a session is valid based on the session ID, ensuring the session exists
    /// and the access token has not expired.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the session to validate.</param>
    /// <returns>A boolean indicating whether the session is valid. Returns true if valid, otherwise false.</returns>
    Task<bool> IsSessionValidAsync(Guid sessionId);
}