using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi.Configs;

/// <summary>
/// Represents the configuration settings required for JWT authentication and token management.
/// </summary>
public class JwtConfiguration(
    string secretKey, 
    string issuer, 
    string audience,
    int accessExpiresInMinutes,
    int refreshExpiresInDays)
{
    /// <summary>
    /// Represents a secret key used for signing and validating JWT tokens.
    /// </summary>
    public string SecretSecretKey { get; } = secretKey;

    /// <summary>
    /// Gets the issuer of the JWT (JSON Web Token), which typically identifies the principal
    /// that issued and signed the token. This value is used during token validation to ensure
    /// the token was issued by a trusted authority.
    /// </summary>
    public string Issuer { get; } = issuer;

    /// <summary>
    /// Gets the audience for which the JSON Web Token (JWT) is intended.
    /// </summary>
    /// <remarks>
    /// The Audience property specifies the valid recipient(s) of the token. It is used to ensure
    /// that the token is being presented to the correct API or service.
    /// Validation against this property helps prevent the JWT from being used by unintended or unauthorized clients.
    /// </remarks>
    public string Audience { get; } = audience;

    /// <summary>
    /// Gets the duration, in minutes, for which the generated access token is valid.
    /// This value determines the expiration time of access tokens issued for authentication requests.
    /// </summary>
    public int AccessExpiresInMinutes { get; } = accessExpiresInMinutes;

    /// <summary>
    /// Gets the number of days until the refresh token expires.
    /// This value defines the validity period of a refresh token in days.
    /// </summary>
    public int RefreshExpiresInDays { get; } = refreshExpiresInDays;

    /// <summary>
    /// Retrieves the token validation parameters required to validate JWT tokens.
    /// </summary>
    /// <returns>A <see cref="TokenValidationParameters"/> instance configured with the necessary settings to validate the token issuer, audience, signing key, and other properties.</returns>
    public TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretSecretKey)),
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}