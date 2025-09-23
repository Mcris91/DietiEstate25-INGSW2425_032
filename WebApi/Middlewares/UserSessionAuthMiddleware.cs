using DietiEstate.WebApi.Services.Interfaces;

namespace DietiEstate.WebApi.Middlewares;

public class UserSessionAuthMiddleware(
    RequestDelegate next,
    IServiceProvider serviceProvider,
    ILogger<UserSessionAuthMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<UserSessionAuthMiddleware> _logger = logger;
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip for anonymus requests
        // if (!context.Request.Path.StartsWithSegments("/api") || 
        //     context.Request.Path.StartsWithSegments("/api/auth/login"))
        // {
        //     await _next(context);
        //     return;
        // }
        
        var sessionId = context.Request.Cookies["session_id"];
        if (string.IsNullOrEmpty(sessionId))
        {
            await _next(context);
            return;
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sessionService = scope.ServiceProvider.GetRequiredService<IUserSessionService>();
        
        var sessionData = await sessionService.GetSessionAsync(new Guid(sessionId));
        if (sessionData == null)
        {
            context.Response.Cookies.Delete("session_id");
            await _next(context);
            return;
        }
        
        if (sessionData.AccessTokenExpiry <= DateTime.UtcNow)
        {
            var refreshSuccess = await sessionService.RefreshSessionTokensAsync(new Guid(sessionId));
            if (!refreshSuccess)
            {
                context.Response.Cookies.Delete("session_id");
                await _next(context);
                return;
            }
            
            sessionData = await sessionService.GetSessionAsync(new Guid(sessionId));
        }
        
        context.Request.Headers.Authorization = $"Bearer {sessionData!.AccessToken}";
        
        await _next(context);
    }
}