using System.Text.Json;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Interfaces;
using StackExchange.Redis;

namespace DietiEstate.WebApi.Services.Implementations;

public class RedisSessionService(
    IConnectionMultiplexer connection,
    IUserRepository userRepository,
    ILogger<RedisSessionService> logger,
    IJwtService jwtService,
    JwtConfiguration jwtConfiguration)
    : IUserSessionService
{
    private readonly IDatabase _redis = connection.GetDatabase();
    private readonly TimeSpan _sessionTtl = TimeSpan.FromDays(30); // TTL della sessione

    public async Task<string> CreateSessionAsync(User user, string accessToken, string refreshToken)
    {
        var session = new UserSession()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(jwtConfiguration.AccessExpiresInMinutes),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(jwtConfiguration.RefreshExpiresInDays),
            CreatedAt = DateTime.UtcNow,
            LastAccessed = DateTime.UtcNow
        };
        await _redis.StringSetAsync($"session:{session.Id}", JsonSerializer.Serialize(session), _sessionTtl);

        await _redis.SetAddAsync($"user_sessions:{user.Id}", session.Id.ToString());
        await _redis.SetAddAsync($"user_sessions:{user.Id}", _sessionTtl.ToString());
        
        logger.LogInformation("Created session {SessionId} for user {UserId}", session.Id, user.Id);
        
        return session.Id.ToString();
    }

    public async Task<UserSession?> GetSessionAsync(Guid sessionId)
    {
        var session = await _redis.StringGetAsync($"session:{sessionId}");
        return session.HasValue 
            ? JsonSerializer.Deserialize<UserSession>(session!)
            : null;
    }

    public async Task<bool> RefreshSessionTokensAsync(Guid sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        if (session is null) return false;
        if (session.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            await InvalidateSessionAsync(sessionId);
            return false;
        }

        var user = await userRepository.GetUserByIdAsync(session.UserId);
        if (user is null)
        {
            await InvalidateSessionAsync(sessionId);
            return false;
        }

        try
        {
            var accessToken = jwtService.GenerateJwtAccessToken(user);
            session.AccessToken = accessToken;
            session.AccessTokenExpiry = DateTime.UtcNow.AddMinutes(jwtConfiguration.AccessExpiresInMinutes);
            session.LastAccessed = DateTime.UtcNow;

            await _redis.StringSetAsync($"session:{sessionId}", JsonSerializer.Serialize(session), _sessionTtl);
            
            logger.LogInformation("Refreshed tokens for session {SessionId}", sessionId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to refresh tokens for session {SessionId}", sessionId);
            await InvalidateSessionAsync(sessionId);
            return false;
        }
    }

    public async Task InvalidateSessionAsync(Guid sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        if (session != null)
        {
            await _redis.SetRemoveAsync($"user_sessions:{session.UserId}", sessionId.ToString());
        }
        
        await _redis.KeyDeleteAsync($"session:{sessionId}");
        logger.LogInformation("Invalidated session {SessionId}", sessionId);
    }

    public async Task InvalidateAllUserSessionsAsync(Guid userId)
    {
        var sessionIds = await _redis.SetMembersAsync($"user_sessions:{userId}");
        
        foreach (var sessionId in sessionIds)
        {
            await _redis.KeyDeleteAsync($"session:{sessionId}");
        }
        
        await _redis.KeyDeleteAsync($"user_sessions:{userId}");
        logger.LogInformation("Invalidated all sessions for user {UserId}", userId);
    }

    public async Task<bool> IsSessionValidAsync(Guid sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        return session is not null && session.RefreshTokenExpiry > DateTime.UtcNow;
    }
}