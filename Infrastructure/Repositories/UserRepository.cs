using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Infrastructure.Data;
using DietiEstate.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastructure.Repositories;

public class UserRepository(
    DietiEstateDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User?>> GetUsersAsync(UserFilterDto filters, int? pageNumber, int? pageSize)
    {
        return await context.User
            .ApplyFilters(filters)
            .ApplySorting(filters.SortBy, filters.SortOrder)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await context.User.FindAsync(userId);
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.User.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task AddUserAsync(User user)
    {
        await context.Database.BeginTransactionAsync();
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        await context.Database.BeginTransactionAsync();
        context.User.Update(user);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        await context.Database.BeginTransactionAsync();
        context.User.Remove(user);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();   
    }
}