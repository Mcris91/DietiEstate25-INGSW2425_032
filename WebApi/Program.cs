using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Data;
using DietiEstate.WebApi.Handlers;
using DietiEstate.WebApi.Repositories.Implementations;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Implementations;
using DietiEstate.WebApi.Services.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.WebApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Env.Load();
        
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        ConfigureAuthentication(builder);
        ConfigureAuthorization(builder);
        
        var app = builder.Build();
        await ConfigureApplicationAsync(app);

        await app.RunAsync();
    }
    
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DietiEstateDbContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"), dboptions =>
            {
                dboptions.MapEnum<UserRole>("user_role")
                    .EnableRetryOnFailure();
                dboptions.EnableRetryOnFailure(0);
            });
        }, ServiceLifetime.Transient);
        
        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<JwtSecuritySchemeTransformer>();
        });
        
        builder.Services.AddScoped<IListingRepository, ListingRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        
        builder.Services.Configure<AuthConfig>(
            builder.Configuration.GetSection("Authentication"));
        
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
        var jwtConfig = new JwtConfiguration(
            Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "secret",
            Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "secret",
            Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "secret",
            int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_MINUTES_EXPIRY") ?? "15"),
            int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_DAYS_EXPIRY") ?? "30")
        );
        builder.Services.AddSingleton(jwtConfig);
        
        var authConfig = builder.Configuration.GetSection("Authentication").Get<AuthConfig>();
        if (authConfig?.BypassAuth == true)
        {
            builder.Services.AddAuthentication("BypassAuth")
                .AddScheme<AuthenticationSchemeOptions, BypassAuthHandler>("BypassAuth", _ => { });
            Console.WriteLine("🚧 DEVELOPMENT MODE: Authentication bypassed!");
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
        builder.Services.AddScoped<IAuthorizationHandler, MinimumRoleHandler>();
        if (authConfig?.BypassAuth == true)
        {
            builder.Services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("BypassAuth")
                    .RequireAssertion(_ => true)
                    .Build())
                .AddPolicy("SuperAdminOnly", policy => 
                    policy.RequireAssertion(_ => true))
                .AddPolicy("SupportOrSuperAdmin", policy =>
                    policy.RequireAssertion(_ => true))
                .AddPolicy("SupportAdminOnly", policy =>
                    policy.RequireAssertion(_ => true))
                .AddPolicy("AgentOnly", policy =>
                    policy.RequireAssertion(_ => true))
                .AddPolicy("MinimumClient", policy => 
                    policy.RequireAssertion(_ => true))
                .AddPolicy("MinimumAgent", policy => 
                    policy.RequireAssertion(_ => true))
                .AddPolicy("MinimumSupportAdmin", policy => 
                    policy.RequireAssertion(_ => true));
        }
        else
        {
            builder.Services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new MinimumRoleRequirement(UserRole.Client))
                    .Build())
                .AddPolicy("SuperAdminOnly", policy =>
                    policy.RequireRole(nameof(UserRole.SuperAdmin)))
                .AddPolicy("SupportOrSuperAdmin", policy =>
                    policy.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin)))
                .AddPolicy("SupportAdminOnly", policy =>
                    policy.RequireRole(nameof(UserRole.Admin)))
                .AddPolicy("AgentOnly", policy =>
                    policy.RequireRole(nameof(UserRole.Agent)))
                .AddPolicy("MinimumClient", policy => 
                    policy.Requirements.Add(new MinimumRoleRequirement(UserRole.Client)))
                .AddPolicy("MinimumAgent", policy => 
                    policy.Requirements.Add(new MinimumRoleRequirement(UserRole.Agent)))
                .AddPolicy("MinimumSupportAdmin", policy => 
                    policy.Requirements.Add(new MinimumRoleRequirement(UserRole.Admin)));
        }
    }
    
    private static async Task ConfigureApplicationAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        if (!app.Environment.IsStaging())
        {
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DietiEstateDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        
        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}