using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.ListingModels;
using DietiEstate.WebApi.Data;
using DietiEstate.WebApi.Extensions;
using DietiEstate.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.WebApi.Services;

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

    public async Task<Listing?> AddListingAsync(Listing listing)
    {
        throw new System.NotImplementedException();
    }
    
    public async Task<Listing?> UpdateListingAsync(Listing listing)
    {
        throw new System.NotImplementedException();
    }
    
    public async Task<Listing?> DeleteListingAsync(Listing listing)
    {
        throw new System.NotImplementedException();
    }
}