using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Data;
using DietiEstate.WebApi.Extensions;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.WebApi.Repositories.Implementations;

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
        throw new NotImplementedException();
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
    
    public async Task AddUserAsync(User user)
    {
        user.Id = Guid.NewGuid();
        await context.Database.BeginTransactionAsync();
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}