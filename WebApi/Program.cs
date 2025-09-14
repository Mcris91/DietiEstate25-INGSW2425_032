using System.Text;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Repositories.Implementations;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        ConfigureAuthentication(builder);
        
        var app = builder.Build();
        ConfigureApplicationAsync(app);

        app.Run();
    }
    
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        builder.Services.AddOpenApi();
        
        builder.Services.AddScoped<IListingRepository, ListingRepository>();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IPasswordService, BCryptPasswordService>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey("aaa"u8.ToArray()),
                ValidateIssuer = true,
                ValidIssuer = "localhost",
                ValidateAudience = true,
                ValidAudience = "users",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
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