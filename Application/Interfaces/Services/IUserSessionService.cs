using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Application.Interfaces.Services;

public interface IUserSessionService
{
    Task<string> CreateSessionAsync(User user, string accessToken, string refreshToken);

    Task<UserSession?> GetSessionAsync(Guid sessionId);

    Task<bool> RefreshSessionTokensAsync(Guid sessionId);

    Task InvalidateSessionAsync(Guid sessionId);

    Task InvalidateAllUserSessionsAsync(Guid userId);

    Task<bool> IsSessionValidAsync(Guid sessionId);
}