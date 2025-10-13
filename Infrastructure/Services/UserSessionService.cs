using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Infrastracture.Config;
using DietiEstate.Infrastracture.Data;

namespace DietiEstate.Infrastracture.Services;

public class UserSessionService(
    DietiEstateDbContext context,
    IUserRepository userRepository,
    IJwtService jwtService,
    JwtConfiguration jwtConfiguration) : IUserSessionService
{
    public async Task<string> CreateSessionAsync(User user, string accessToken, string refreshToken)
    {
        var userSession = new UserSession()
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
        await context.UserSession.AddAsync(userSession);
        await context.SaveChangesAsync();
        return userSession.Id.ToString();
    }

    public async Task<UserSession?> GetSessionAsync(Guid sessionId)
    {
        return await context.UserSession.FindAsync(sessionId);
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
            await context.Database.BeginTransactionAsync();
            context.UserSession.Update(session);
            await context.SaveChangesAsync();
            await context.Database.CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {
            await InvalidateSessionAsync(sessionId);
            return false;
        }
    }

    public async Task InvalidateSessionAsync(Guid sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        if (session is null) return;
        
        await context.Database.BeginTransactionAsync();
        context.UserSession.Remove(session);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();   
    }

    public async Task InvalidateAllUserSessionsAsync(Guid userId)
    {
        var sessions = context.UserSession.Where(s => s.UserId == userId);
        if (!sessions.Any()) return;

        await context.Database.BeginTransactionAsync();
        context.UserSession.RemoveRange(sessions);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task<bool> IsSessionValidAsync(Guid sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        if (session is null) return false;
        return session.AccessTokenExpiry > DateTime.UtcNow;   
    }
}