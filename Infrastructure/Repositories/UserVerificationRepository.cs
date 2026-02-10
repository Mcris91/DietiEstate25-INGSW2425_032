using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastructure.Repositories;

public class UserVerificationRepository(
    DietiEstateDbContext context): IUserVerificationRepository
{
    public async Task<UserVerification?> GetVerificationByTokenAsync(string token)
    {
        return await context.UserVerification.FirstOrDefaultAsync(v => v.Token == token);
    }

    public async Task<UserVerification?> GetVerificationByUserIdAsync(Guid userId)
    {
        return await context.UserVerification.FirstOrDefaultAsync(v => v.UserId == userId);
    }

    public async Task AddVerificationAsync(UserVerification verification)
    {
        verification.Id = Guid.NewGuid();
        await context.Database.BeginTransactionAsync();
        await context.UserVerification.AddAsync(verification);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();   
    }

    public async Task UpdateVerificationAsync(UserVerification verification)
    {
        await context.Database.BeginTransactionAsync();
        context.UserVerification.Update(verification);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task DeleteVerificationAsync(UserVerification verification)
    {
        await context.Database.BeginTransactionAsync();
        context.UserVerification.Remove(verification);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}