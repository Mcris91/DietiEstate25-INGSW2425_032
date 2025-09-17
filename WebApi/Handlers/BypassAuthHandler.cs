using System.Security.Claims;
using System.Text.Encodings.Web;
using DietiEstate.WebApi.Configs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DietiEstate.WebApi.Handlers;

/// <summary>
/// The <c>BypassAuthHandler</c> class is a custom authentication handler designed for development
/// environments where authentication can be bypassed for testing or debugging purposes. It creates
/// a fake authenticated user based on predefined settings in the <see cref="AuthConfig"/> class.
/// </summary>
/// <remarks>
/// This handler is intended to override standard authentication mechanisms by generating a user
/// identity with basic claims information such as username, email, and roles. It simplifies
/// working in local or development environments by avoiding the need for complex authentication flows.
/// </remarks>
/// <example>
/// To enable the bypass authentication, ensure the <c>BypassAuth</c> feature in <c>AuthConfig</c>
/// is set to true. This handler generates a mock identity whenever authentication is required.
/// </example>
/// <seealso cref="AuthConfig"/>
public class BypassAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<AuthConfig> authConfig)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly AuthConfig _authConfig = authConfig.Value;
    private readonly ILogger<BypassAuthHandler> _logger = logger.CreateLogger<BypassAuthHandler>();

    /// <summary>
    /// Authenticates a request by creating a fake user identity when the authentication bypass mode is enabled.
    /// </summary>
    /// <returns>
    /// A successful authentication result containing a generated authentication ticket with predefined claims
    /// if the bypass authentication option is enabled. Otherwise, returns no authentication result.
    /// </returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!_authConfig.BypassAuth)
        {
            _logger.LogDebug("BypassAuth is disabled, no authentication result");
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        _logger.LogInformation("🚧 DEVELOPMENT: Bypassing authentication with fake user (Role: {Role})", 
            _authConfig.DefaultRole);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _authConfig.DefaultUserId),
            new Claim(ClaimTypes.Name, _authConfig.DefaultUsername),
            new Claim(ClaimTypes.Email, _authConfig.DefaultEmail),
            new Claim(ClaimTypes.Role, _authConfig.DefaultRole)
        };

        var identity = new ClaimsIdentity(claims, "BypassAuth");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "BypassAuth");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}