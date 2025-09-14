using System.Security.Claims;
using DietiEstate.Shared.Models.AuthModels;

namespace DietiEstate.WebApi.Services;

public interface IJwtService
{
    string GenerateJwtAccessToken(string userId);
    string GenerateJwtRefreshToken(string userId);
    string GenerateJwtToken(string userId, JwtTokenType tokenType, int tokenExpiryInMinutes);

    ClaimsPrincipal? ValidateJwtAccessToken(string token);
    ClaimsPrincipal? ValidateJwtRefreshToken(string token);
    ClaimsPrincipal? ValidateJwtToken(string token);
}