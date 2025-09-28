using System.Text.Json;
using DietiEstate.WebApi.Services.Interfaces;
using StackExchange.Redis;

namespace DietiEstate.WebApi.Services.Implementations;

public class PasswordResetService(
    IConnectionMultiplexer connection,
    ILogger<PasswordResetService> logger
    ) : IPasswordResetService
{
    private readonly IDatabase _redis = connection.GetDatabase();
    private readonly TimeSpan _sessionTtl = TimeSpan.FromMinutes(10);

    public async Task<string> GetResetRequestEmailAsync(Guid resetTokenId)
    {
        var requestEmail = await _redis.StringGetAsync($"session:{resetTokenId}");

        return requestEmail.HasValue
            ? requestEmail!
            : "";
    }

    public async Task<bool> CreatePasswordResetRequestAsync(string email)
    {
        try
        {
            var resetTokenId = Guid.NewGuid();
            await _redis.StringSetAsync(
                $"password_reset_request:{resetTokenId.ToString()}",
                JsonSerializer.Serialize(new
                {
                    Id = resetTokenId,
                    Email = email
                }), _sessionTtl);
            logger.LogInformation("Password reset request for email {Email}", email);   
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create a password reset request for email {Email}", email);
        }

        return true;
    }

    public async Task<bool> ValidateResetTokenAsync(Guid resetTokenId)
    {
        var resetRequest = await GetResetRequestEmailAsync(resetTokenId);
        return resetRequest != "";
    }
}