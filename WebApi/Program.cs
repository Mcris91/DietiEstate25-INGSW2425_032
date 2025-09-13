using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Data;
using DietiEstate.WebApi.Repositories;
using DietiEstate.WebApi.Services;
using Microsoft.EntityFrameworkCore;

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

        builder.Services.AddScoped<IListingRepository, ListingRepository>();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        builder.Services.AddDbContext<DietiEstateDbContext>(options =>
        {
            _ = options.UseNpgsql("", options =>
            {
                _ = options.MapEnum<UserRole>("user_role");
            });
        });
        
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