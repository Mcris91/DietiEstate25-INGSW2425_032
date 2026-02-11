using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastructure.Config;
using DietiEstate.Infrastructure.Data;
using DietiEstate.Infrastructure.Repositories;
using DietiEstate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DietiEstate.Infrastructure;

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
                dboptions.UseNetTopologySuite();
                dboptions.MapEnum<UserRole>("user_role")
                    .EnableRetryOnFailure();
                dboptions.EnableRetryOnFailure(0);
            });
        }, ServiceLifetime.Transient);
        
        services.AddScoped<IAgencyRepository, AgencyRepository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserVerificationRepository, UserVerificationRepository>();
        services.AddScoped<IFavouritesRepository, FavouritesRepository>();
        services.AddScoped<ITestRepository, TestRepository>();
        
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, BCryptPasswordService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IUserSessionService, RedisSessionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IExcelService, ExcelService>();
        
        
        
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddAutoMapper(typeof(AutoMapperProfile));

        return services;
    }
}