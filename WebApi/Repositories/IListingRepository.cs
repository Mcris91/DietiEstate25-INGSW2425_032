using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.WebApi.Repositories;

public interface IListingRepository
{
    Task<IEnumerable<Listing?>> GetListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize);
    Task<Listing?> GetListingByIdAsync(Guid listingId);
}