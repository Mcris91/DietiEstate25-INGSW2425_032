using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastructure.Data;
using DietiEstate.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace DietiEstate.Infrastructure.Repositories;

public class ListingRepository(DietiEstateDbContext context) : IListingRepository
{
    public async Task<IEnumerable<Listing>> GetListingsAsync(ListingFilterDto filters)
    {
        return await context.Listing
            .Include(l => l.Type)
            .Include(l => l.ListingTags)
            .ApplyFilters(filters)
            .ApplyNumericFilters(filters)
            .ApplySorting(filters.SortBy, filters.SortOrder, new Point(filters.Longitude.Value, filters.Latitude.Value) { SRID = 4326 })
            .ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetRecentListingsAsync(List<Guid> listingIdsList)
    {
        return await context.Listing
            .Include(l => l.Type)
            .Include(l => l.ListingTags)
            .Where(l => listingIdsList.Contains(l.Id))
            .ToListAsync();
    }
    public async Task<IEnumerable<Listing>> GetDetailedListingsAsync(ListingFilterDto filters)
    {
        return await context.Listing
            .Include(l => l.Type)
            .Include(l => l.ListingServices)
            .Include(l => l.ListingTags)
            //.Include(l => l.ListingImages)
            .Include(l => l.ListingOffers)
            .Include(l => l.ListingBookings)
            .Include(l => l.ListingFavourites)
            .Include(l => l.Agent)
            .ApplyFilters(filters)
            .ApplyNumericFilters(filters)
            .ApplySorting(filters.SortBy, filters.SortOrder, new Point(filters.Longitude.Value, filters.Latitude.Value) { SRID = 4326 })
            .ToListAsync();
    }
    
    public async Task<Listing?> GetListingByIdAsync(Guid listingId)
    {
        return await context.Listing
            .Include(l => l.Type)
            .Include(l => l.ListingServices)
            .Include(l => l.ListingTags)
            .Include(l => l.ListingImages)
            .Include(l => l.Agent)
            .FirstOrDefaultAsync(l => l.Id == listingId);
    }

    public async Task AddListingAsync(Listing listing, List<string> tags)
    {
        listing.ListingTags = await context.Tag
            .Where(t => tags.Contains(t.Name))
            .ToListAsync();
        
        await context.Database.BeginTransactionAsync();
        await context.Listing.AddAsync(listing);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
    
    public async Task UpdateListingAsync(Listing listing, List<string>? tags)
    {
        if (tags != null)
        {
            listing.ListingTags = await context.Tag
                .Where(t => tags.Contains(t.Name))
                .ToListAsync();
        }
        
        await context.Database.BeginTransactionAsync();
        context.Listing.Update(listing);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
    
    public async Task DeleteListingAsync(Listing listing)
    {
        await context.Database.BeginTransactionAsync();
        context.Listing.Remove(listing);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}