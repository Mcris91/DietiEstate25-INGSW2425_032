using AutoMapper;
using DietiEstate.Shared.Enums;
using DietiEstate.Shared.Models.Configs;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.WebApi.Data.Seeders;

public class DatabaseSeeder(
    IMapper mapper,
    IConfiguration configuration,
    DietiEstateDbContext context, 
    IPasswordService passwordService,
    ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAsync()
    {
        await SeedSystemAdminAsync();
    }

    private async Task SeedSystemAdminAsync()
    {
        var adminUsers = configuration.GetSection("DefaultUsers:SystemAdmins").Get<List<SystemAdminConfig>>();
        if (adminUsers is null)
        {
            logger.LogWarning("No system admin users found in configuration. Skipping database seeding.");
            return;
        }
        
        var adminEmails = adminUsers.Select(u => u.Email.ToLowerInvariant()).ToList();
        var existingEmails = await context.User
            .Where(u => adminEmails.Contains(u.Email.ToLower()))
            .Select(u => u.Email.ToLower())
            .ToListAsync();

        adminUsers = adminUsers
            .Where(u => !existingEmails.Contains(u.Email.ToLowerInvariant()))
            .ToList();

        if (adminUsers.Count == 0)
        {
            logger.LogInformation("All admin users found in configuration already exist in the database. Skipping database seeding.");
            return;
        }
      
        await context.Database.BeginTransactionAsync();
        try
        {
            var users = mapper.Map<List<User>>(adminUsers);
            users.ForEach(user =>
            {
                user.Password = passwordService.HashPassword(user.Password);
                user.Role = UserRole.SystemAdmin;
            });
            await context.User.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot seed system admins to database.");
            return;
        }
        await context.Database.CommitTransactionAsync();
        logger.LogInformation("Seeding system admins to database completed.");
    }
}