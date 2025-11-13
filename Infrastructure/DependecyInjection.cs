using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastracture.Config;
using DietiEstate.Infrastracture.Data;
using DietiEstate.Infrastracture.Repositories;
using DietiEstate.Infrastracture.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DietiEstate.Infrastracture;

public static class DependecyInjection
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddDbContext<DietiEstateDbContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"), dboptions =>
            {
                dboptions.MapEnum<UserRole>("user_role")
                    .EnableRetryOnFailure();
                dboptions.EnableRetryOnFailure(0);
            });
        }, ServiceLifetime.Transient);
        
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserVerificationRepository, UserVerificationRepository>();
        
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, BCryptPasswordService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IUserSessionService, RedisSessionService>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddAutoMapper(typeof(AutoMapperProfile));

        return services;
    }
}