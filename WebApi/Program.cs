using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Constants;
using DietiEstate.Infrastructure;
using DietiEstate.Infrastructure.Config;
using DietiEstate.Infrastructure.Data;
using DietiEstate.Infrastructure.Data.Seeders;
using DietiEstate.Infrastructure.Services;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Handlers;
using DietiEstate.WebApi.Middlewares;
using DotNetEnv;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Minio;
using StackExchange.Redis;

namespace DietiEstate.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }
        
        var builder = WebApplication.CreateBuilder(args);
        
        ConfigureMinio(builder);
        ConfigureServices(builder);

        ConfigureAuthentication(builder);
        ConfigureAuthorization(builder);
        
        var app = builder.Build();
        await ConfigureApplicationAsync(app);

        await app.RunAsync();
    }
    
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins", policy =>
            {
                policy.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        });
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!));
        
        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<JwtSecuritySchemeTransformer>();
        });
        
        builder.Services.AddScoped<DatabaseSeeder>();

        builder.Services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UsePostgreSqlStorage(options => { 
                    options.UseNpgsqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
                });
        });

        builder.Services.AddScoped<IMinioService, MinioService>();
        
        builder.Services.AddHttpClient<GeoapifyService>(client => 
        {
            client.BaseAddress = new Uri("https://api.geoapify.com/v2/");
        });
        
        builder.Services.AddScoped<IMinioClient>(sp => 
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var endpoint = config["MINIO_ENDPOINT"] ?? "localhost:9000";
            var accessKey = config["MINIO_ACCESS_KEY"] ?? "minioadmin";
            var secretKey = config["MINIO_SECRET_KEY"] ?? "minioadmin";

            return new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .Build();
        });
        
        builder.Services.AddDependencies(builder.Configuration);
        
        builder.Services.Configure<AuthConfig>(
            builder.Configuration.GetSection("Authentication"));
        
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static void ConfigureMinio(WebApplicationBuilder builder)
    {
        var minioConfig = new MinioConfiguration(
            Environment.GetEnvironmentVariable("MINIO_ENDPOINT") ?? "localhost:9000",
            Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY") ?? "minioadmin",
            Environment.GetEnvironmentVariable("MINIO_SECRET_KEY") ?? "minioadmin",
            Environment.GetEnvironmentVariable("MINIO_BUCKET") ?? "listingbucket"
        );
        builder.Services.AddSingleton(minioConfig);
    }
    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        var jwtConfig = new JwtConfiguration(
            Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "secret",
            Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "secret",
            Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "secret",
            int.Parse(Environment.GetEnvironmentVariable("JWT_ID_MINUTES_EXPIRY") ?? "60"),
            int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_MINUTES_EXPIRY") ?? "15"),
            int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_DAYS_EXPIRY") ?? "30")
        );
        builder.Services.AddSingleton(jwtConfig);
        
        var authConfig = builder.Configuration.GetSection("Authentication").Get<AuthConfig>();
        if (authConfig?.BypassAuth == true)
        {
            builder.Services.AddAuthentication("BypassAuth")
                .AddScheme<AuthenticationSchemeOptions, BypassAuthHandler>("BypassAuth", null);
        }
        else
        {
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = jwtConfig.GetTokenValidationParameters();
                    jwtOptions.MapInboundClaims = false;
                });
        }
    }
    
    private static void ConfigureAuthorization(WebApplicationBuilder builder)
    {
        var authConfig = builder.Configuration.GetSection("Authentication").Get<AuthConfig>();
        if (authConfig?.BypassAuth == true)
        {
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("ReadListing", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteListing", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteListing", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("ReadAgent", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteAgent", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteAgent", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("ReadSupportAdmin", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteSupportAdmin", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteSupportAdmin", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("ReadUser", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteUser", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteUser", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("ReadOffer", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteOffer", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteOffer", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("ReadBooking", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteBooking", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteBooking", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("ReadFavourite", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("WriteFavourite", policy =>
                    policy.RequireAssertion( _ => true))
                .AddPolicy("DeleteFavourite", policy =>
                    policy.RequireAssertion( _ => true));
        }
        else
        {
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("SystemAdmin", policy =>
                    policy.RequireClaim("scope", UserScope.SystemAdmin))
                .AddPolicy("ReadListing", policy =>
                    policy.RequireClaim("scope", UserScope.ReadListing))
                .AddPolicy("WriteListing", policy =>
                    policy.RequireClaim("scope", UserScope.WriteListing))
                .AddPolicy("DeleteListing", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteListing))
                .AddPolicy("ReadAgent", policy =>
                    policy.RequireClaim("scope", UserScope.ReadAgent))
                .AddPolicy("WriteAgent", policy =>
                    policy.RequireClaim("scope", UserScope.WriteAgent))
                .AddPolicy("DeleteAgent", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteAgent))
                .AddPolicy("ReadSupportAdmin", policy =>
                    policy.RequireClaim("scope", UserScope.ReadSupportAdmin))
                .AddPolicy("WriteSupportAdmin", policy =>
                    policy.RequireClaim("scope", UserScope.WriteSupportAdmin))
                .AddPolicy("DeleteSupportAdmin", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteSupportAdmin))
                .AddPolicy("ReadUser", policy =>
                    policy.RequireClaim("scope", UserScope.ReadUser))
                .AddPolicy("WriteUser", policy =>
                    policy.RequireClaim("scope", UserScope.WriteUser))
                .AddPolicy("DeleteUser", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteUser))
                .AddPolicy("ReadOffer", policy =>
                    policy.RequireClaim("scope", UserScope.ReadOffer))
                .AddPolicy("WriteOffer", policy =>
                    policy.RequireClaim("scope", UserScope.WriteOffer))
                .AddPolicy("DeleteOffer", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteOffer))
                .AddPolicy("ReadBooking", policy =>
                    policy.RequireClaim("scope", UserScope.ReadBooking))
                .AddPolicy("WriteBooking", policy =>
                    policy.RequireClaim("scope", UserScope.WriteBooking))
                .AddPolicy("DeleteBooking", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteBooking))
                .AddPolicy("ReadFavourite", policy =>
                    policy.RequireClaim("scope", UserScope.ReadFavourite))
                .AddPolicy("WriteFavourite", policy =>
                    policy.RequireClaim("scope", UserScope.WriteFavourite))
                .AddPolicy("DeleteFavourite", policy =>
                    policy.RequireClaim("scope", UserScope.DeleteFavourite));
        }
    }
    
    private static async Task ConfigureApplicationAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHangfireDashboard();

        if (!app.Environment.IsStaging())
        {
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DietiEstateDbContext>();
            await dbContext.Database.MigrateAsync();

            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowedOrigins");
        app.UseMiddleware<UserSessionAuthMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
