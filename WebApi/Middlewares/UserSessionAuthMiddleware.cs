using DietiEstate.Application.Interfaces.Services;

namespace DietiEstate.WebApi.Middlewares;

public class UserSessionAuthMiddleware(
    RequestDelegate next,
    IServiceProvider serviceProvider)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip for anonymus requests
        if (context.Request.Path.StartsWithSegments("/api/v1/Auth/login"))
        {
            await next(context);
            return;
        }
        
        var sessionId = context.Request.Cookies["session_id"];
        if (string.IsNullOrEmpty(sessionId))
        {
            await next(context);
            return;
        }
        
        using var scope = serviceProvider.CreateScope();
        var sessionService = scope.ServiceProvider.GetRequiredService<IUserSessionService>();
        
        var sessionData = await sessionService.GetSessionAsync(new Guid(sessionId));
        if (sessionData == null)
        {
            context.Response.Cookies.Delete("session_id");
            context.Response.Cookies.Delete("id_token");
            await next(context);
            return;
        }
        
        if (sessionData.AccessTokenExpiry <= DateTime.UtcNow)
        {
            var refreshSuccess = await sessionService.RefreshSessionTokensAsync(new Guid(sessionId));
            if (!refreshSuccess)
            {
                context.Response.Cookies.Delete("session_id");
                context.Response.Cookies.Delete("id_token");
                await next(context);
                return;
            }
            
            sessionData = await sessionService.GetSessionAsync(new Guid(sessionId));
        }
        
        context.Request.Headers.Authorization = $"Bearer {sessionData!.AccessToken}";
        
         await next(context);
    }
}