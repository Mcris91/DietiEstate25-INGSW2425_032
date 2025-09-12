using DietiEstate.WebApi.Configs;

namespace DietiEstate.WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        
        var app = builder.Build();
        ConfigureApplicationAsync(app);

        app.Run();
    }
    
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static void ConfigureApplicationAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}