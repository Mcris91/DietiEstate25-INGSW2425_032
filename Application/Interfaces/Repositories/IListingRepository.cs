using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IListingRepository
{
    Task<IEnumerable<Listing?>> GetListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize);

    Task<Listing?> GetListingByIdAsync(Guid listingId);

    Task AddListingAsync(Listing listing, List<Guid> services, List<Guid> tags, List<string> images);

    Task UpdateListingAsync(Listing listing);

    Task DeleteListingAsync(Listing listing);
}