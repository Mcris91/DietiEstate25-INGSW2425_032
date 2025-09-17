namespace DietiEstate.WebApi.Configs;

/// <summary>
/// The <c>AuthConfig</c> class defines configuration settings for authentication mechanisms
/// within the application. These settings are typically loaded from configuration files or
/// environment variables and used to manage authentication flows and default behavior.
/// </summary>
public class AuthConfig
{
    /// <summary>
    /// If true, bypasses JWT authentication and creates a fake authenticated user.
    /// Should only be used in development environments.
    /// </summary>
    public bool BypassAuth { get; init; } = false;

    /// <summary>
    /// Default role assigned to the fake user when BypassAuth is enabled.
    /// </summary>
    public string DefaultRole { get; init; } = "Client";

    /// <summary>
    /// Default user ID for the fake user when BypassAuth is enabled.
    /// </summary>
    public string DefaultUserId { get; init; } = "dev-user-123";

    /// <summary>
    /// Default username for the fake user when BypassAuth is enabled.
    /// </summary>
    public string DefaultUsername { get; init; } = "Development User";

    /// <summary>
    /// Default email for the fake user when BypassAuth is enabled.
    /// </summary>
    public string DefaultEmail { get; init; } = "dev@example.com";
}