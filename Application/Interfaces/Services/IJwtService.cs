using System.Security.Claims;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateJwtIdToken(User user);
    
    string GenerateJwtAccessToken(User user);

    string GenerateJwtRefreshToken(User user);

    ClaimsPrincipal? ValidateJwtAccessToken(string token);

    ClaimsPrincipal? ValidateJwtRefreshToken(string token);
}