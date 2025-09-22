using System.Security.Claims;
using DietiEstate.Shared.Models.AuthModels;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Services.Interfaces;

/// <summary>
/// Interface for providing functionalities related to JWT (JSON Web Token) operations,
/// including generating and validating access and refresh tokens.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT (JSON Web Token) id token for the specified user.
    /// </summary>
    /// <param name="user">The user object containing the details of the user for whom the access token is being generated.</param>
    /// <returns>A string representation of the generated JWT access token.</returns>
    string GenerateJwtIdToken(User user);
    
    /// <summary>
    /// Generates a JWT (JSON Web Token) access token for the specified user.
    /// </summary>
    /// <param name="user">The user object containing the details of the user for whom the access token is being generated.</param>
    /// <returns>A string representation of the generated JWT access token.</returns>
    string GenerateJwtAccessToken(User user);

    /// <summary>
    /// Generates a JWT (JSON Web Token) refresh token for the specified user.
    /// The refresh token is leveraged to obtain a new access token after the preceding one expires.
    /// </summary>
    /// <param name="user">The user entity for whom the refresh token is being generated.</param>
    /// <returns>A string containing the generated JWT refresh token.</returns>
    string GenerateJwtRefreshToken(User user);

    /// <summary>
    /// Validates a JWT access token and determines its validity by ensuring the token type is
    /// of type "Access" and it conforms to the JWT validation parameters defined in the configuration.
    /// </summary>
    /// <param name="token">The JWT access token to be validated.</param>
    /// <returns>
    /// A <see cref="ClaimsPrincipal"/> representing the principal extracted from the validated token if
    /// the token is valid and of type "Access"; otherwise, null.
    /// </returns>
    ClaimsPrincipal? ValidateJwtAccessToken(string token);

    /// <summary>
    /// Validates a JWT refresh token and returns the claims principal if the token is valid
    /// and corresponds to a refresh token.
    /// </summary>
    /// <param name="token">The JWT refresh token to be validated.</param>
    /// <returns>
    /// A <see cref="ClaimsPrincipal"/> object representing the claims from the validated refresh token,
    /// or null if the token is invalid, expired, or does not correspond to a refresh token.
    /// </returns>
    ClaimsPrincipal? ValidateJwtRefreshToken(string token);
}