using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IListingRepository
{
    Task<IEnumerable<Listing>> GetDetailedListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize);
    
    Task<IEnumerable<Listing>> GetListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize);

    Task<IEnumerable<Listing>> GetRecentListingsAsync(List<Guid> listingIdsList);    
    Task<Listing?> GetListingByIdAsync(Guid listingId);

    Task AddListingAsync(Listing listing, List<string> tags);

    Task UpdateListingAsync(Listing listing, List<string>? tags);

    Task DeleteListingAsync(Listing listing);
}