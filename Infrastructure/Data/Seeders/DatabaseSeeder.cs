using AutoMapper;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Enums;
using DietiEstate.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace DietiEstate.Infrastracture.Data.Seeders;

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
        await SeedDefaultTagsAsync();
    }

    private async Task SeedSystemAdminAsync()
    {
        var adminUsers = configuration.GetSection("DefaultUsers:SystemAdmins").Get<List<AdminUserTemplate>>();
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

    private async Task SeedDefaultTagsAsync()
    {
        var defaultTags = configuration.GetSection("DefaultTags:Tags").Get<List<DefaultTagTemplate>>();
        if (defaultTags is null)
        {
            logger.LogWarning("No tags found in configuration. Skipping database seeding.");
            return;
        }
        
        var tagNames = defaultTags.Select(t => t.Name.ToLowerInvariant()).ToList();

        var existingTags = await context.Tag
            .Where(t => tagNames.Contains(t.Name.ToLower()))
            .Select(t => t.Name.ToLower())
            .ToListAsync();

        defaultTags = defaultTags
            .Where(t => !existingTags.Contains(t.Name.ToLowerInvariant()))
            .ToList();

        if (defaultTags.Count == 0)
        {
            logger.LogInformation("All default tags found in configuration already exist in the database. Skipping database seeding.");
            return;
        }
      
        await context.Database.BeginTransactionAsync();
        try
        {
            var tags = mapper.Map<List<Tag>>(defaultTags);
            await context.Tag.AddRangeAsync(tags);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot seed default tags to database.");
            return;
        }
        await context.Database.CommitTransactionAsync();
        logger.LogInformation("Seeding default tags to database completed.");
    }
}