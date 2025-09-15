using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Data;
using DietiEstate.WebApi.Repositories.Implementations;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Implementations;
using DietiEstate.WebApi.Services.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        ConfigureAuthentication(builder);
        
        var app = builder.Build();
        await ConfigureApplicationAsync(app);

        await app.RunAsync();
    }
    
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        Env.Load();

        builder.Services.AddDbContext<DietiEstateDbContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"), dboptions =>
            {
                dboptions.MapEnum<UserRole>("user_role")
                    .EnableRetryOnFailure();
            });
        }, ServiceLifetime.Transient);
        
        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        builder.Services.AddOpenApi();
        
        builder.Services.AddScoped<IListingRepository, ListingRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
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
    
    private static async Task ConfigureApplicationAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        if (!app.Environment.IsStaging())
        {
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DietiEstateDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        
        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}