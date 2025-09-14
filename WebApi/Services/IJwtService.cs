using System.Security.Claims;
using DietiEstate.Shared.Models.AuthModels;

namespace DietiEstate.WebApi.Services;

public interface IJwtService
{
    string GenerateJwtAccessToken(string userId);
    string GenerateJwtRefreshToken(string userId);

    ClaimsPrincipal? ValidateJwtAccessToken(string token);
    ClaimsPrincipal? ValidateJwtRefreshToken(string token);
}