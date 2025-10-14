using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastracture.Data;
using DietiEstate.Infrastracture.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Repositories;

public class ListingRepository(DietiEstateDbContext context) : IListingRepository
{
    public async Task<IEnumerable<Listing?>> GetListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize)
    {
        return await context.Listing
            .Include(l => l.ListingServices)
            .Include(l => l.ListingTags)
            .Include(l => l.ListingImages)
            .ApplyFilters(filters)
            .ApplyNumericFilters(filters)
            .ApplySorting(filters.SortBy, filters.SortOrder)
            .ToListAsync();
    }
    
    public async Task<Listing?> GetListingByIdAsync(Guid listingId)
    {
        return await context.Listing
            .Include(l => l.ListingServices)
            .Include(l => l.ListingTags)
            .Include(l => l.ListingImages)
            .FirstOrDefaultAsync(l => l.Id == listingId);
    }

    public async Task AddListingAsync(Listing listing, List<Guid> services, List<Guid> tags, List<string> images)
    {
        // TODO: Add images to the database
        listing.ListingServices = await context.Service
            .Where(s => services.Contains(s.Id))
            .ToListAsync();
        listing.ListingTags = await context.Tag
            .Where(t => tags.Contains(t.Id))
            .ToListAsync();
        listing.ListingImages = await context.Image
            .Where(i => images.Contains(i.Url))
            .ToListAsync();
        
        await context.Database.BeginTransactionAsync();
        await context.Listing.AddAsync(listing);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
    
    public async Task UpdateListingAsync(Listing listing)
    {
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