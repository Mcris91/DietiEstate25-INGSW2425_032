using WebAssemblyClient.ApiService;

namespace WebAssemblyClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiService<TClient>(
        this IServiceCollection services,
        string baseUrl,
        string controllerPath) where TClient : BaseApiService
    {
        if (string.IsNullOrEmpty(baseUrl)) 
            throw new ArgumentNullException(nameof(baseUrl));

        if (string.IsNullOrEmpty(controllerPath)) 
            throw new ArgumentNullException(nameof(controllerPath));
        
        services.AddHttpClient<TClient>(client =>
        {
            if (!baseUrl.EndsWith('/')) baseUrl += "/";
            client.BaseAddress = new Uri($"{baseUrl}{controllerPath}/");
        })
        .AddHttpMessageHandler<CookieHandler>();;

        return services;
    }
}