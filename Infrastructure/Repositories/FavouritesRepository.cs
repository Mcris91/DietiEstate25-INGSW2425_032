using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.FavouritesModels;
using DietiEstate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastructure.Repositories;

public class FavouritesRepository(
    DietiEstateDbContext context) : IFavouritesRepository
{
    public async Task<Favourite?> GetFavouriteAsync(Guid userId, Guid listingId)
    {
        return await context.Favourites.FirstOrDefaultAsync(f => f.UserId == userId && f.ListingId == listingId);
    }

    public async Task<IEnumerable<Favourite>> GetFavouritesAsync(Guid userId)
    {
        return await context.Favourites
            .Where(f => f.UserId == userId)
            .Include(f => f.Listing)
            .ThenInclude(l => l.ListingTags)
            .ToListAsync();
    }
    
    public async Task CreateFavouriteAsync(Favourite favourite)
    {
        await context.Database.BeginTransactionAsync();
        await context.Favourites.AddAsync(favourite);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task DeleteFavouriteAsync(Favourite favourite)
    {
        await context.Database.BeginTransactionAsync();
        context.Favourites.Remove(favourite);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}