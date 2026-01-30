using System.Text.Json;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.AuthModels;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DietiEstate.Infrastracture.Services;

public class PasswordResetService(
    IConnectionMultiplexer connection,
    ILogger<PasswordResetService> logger
    ) : IPasswordResetService
{
    private readonly IDatabase _redis = connection.GetDatabase();
    private readonly TimeSpan _sessionTtl = TimeSpan.FromMinutes(10);

    public async Task<PasswordReset?> GetResetRequestAsync(string email)
    {
        var passwordReset = await _redis.StringGetAsync($"password_reset_request:{email}");

        return passwordReset.HasValue
            ? JsonSerializer.Deserialize<PasswordReset>(passwordReset!)
            : null;
    }

    public async Task<PasswordReset?> CreatePasswordResetRequestAsync(string email)
    {
        var passwordReset = new PasswordReset()
        {
            Email = email,
            Token = new Random().Next(100000, 1000000)
        };
        
        try
        {
            await _redis.StringSetAsync(
                $"password_reset_request:{passwordReset.Email}",
                JsonSerializer.Serialize(passwordReset), _sessionTtl);
            logger.LogInformation("Password reset request for email {Email}", email);   
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create a password reset request for email {Email}", email);
            return null;
        }

        return passwordReset;
    }

    public async Task InvalidateResetTokenAsync(string email)
    {
        await _redis.KeyDeleteAsync($"password_reset_request:{email}");
        logger.LogInformation("Invalidated session {email}", email);
    }
    
    public async Task<bool> ValidateResetTokenAsync(string email, int token)
    {
        var resetRequest = await GetResetRequestAsync(email);
        return resetRequest is not null && resetRequest.Token == token;
    }
}