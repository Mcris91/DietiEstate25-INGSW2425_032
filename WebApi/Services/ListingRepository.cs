using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.ListingModels;
using DietiEstate.WebApi.Repositories;

namespace DietiEstate.WebApi.Services;

public class ListingRepository : IListingRepository
{
    public async Task<IEnumerable<Listing?>> GetListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize)
    {
        throw new NotImplementedException();
    }
    public async Task<Listing?> GetListingByIdAsync(Guid listingId)
    {
        throw new NotImplementedException();
    }
}