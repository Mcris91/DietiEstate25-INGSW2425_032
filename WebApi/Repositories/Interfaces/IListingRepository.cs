using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.WebApi.Repositories.Interfaces;

/// <summary>
/// Represents the contract for managing listing-related data in the system.
/// </summary>
public interface IListingRepository
{
    /// <summary>
    /// Asynchronously retrieves a collection of listings based on the provided filter criteria and optional pagination parameters.
    /// </summary>
    /// <param name="filters">An object containing the filter criteria for listings.</param>
    /// <param name="pageNumber">The page number to retrieve for pagination. If null, no pagination is applied.</param>
    /// <param name="pageSize">The number of listings to retrieve per page for pagination. If null, no pagination is applied.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of listings that match the filter and pagination criteria.</returns>
    Task<IEnumerable<Listing?>> GetListingsAsync(ListingFilterDto filters, int? pageNumber, int? pageSize);

    /// <summary>
    /// Asynchronously retrieves a listing by its unique identifier.
    /// </summary>
    /// <param name="listingId">The unique identifier of the listing to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the listing if found; otherwise, null.</returns>
    Task<Listing?> GetListingByIdAsync(Guid listingId);

    /// <summary>
    /// Asynchronously adds a new listing to the system along with associated services, tags, and images.
    /// </summary>
    /// <param name="listing">An object representing the listing to be added, containing its properties and details.</param>
    /// <param name="services">A list of GUIDs representing the services associated with the listing.</param>
    /// <param name="tags">A list of GUIDs representing the tags associated with the listing.</param>
    /// <param name="images">A list of strings representing the URLs of the images associated with the listing.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddListingAsync(Listing listing, List<Guid> services, List<Guid> tags, List<string> images);

    /// <summary>
    /// Asynchronously updates an existing listing with the provided details.
    /// </summary>
    /// <param name="listing">The updated listing object that includes the new details to be applied.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated listing object if the update was successful; otherwise, null.</returns>
    Task UpdateListingAsync(Listing listing);

    /// <summary>
    /// Asynchronously deletes a specified listing from the system.
    /// </summary>
    /// <param name="listing">The listing to be deleted.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted listing, or null if the deletion was unsuccessful.</returns>
    Task DeleteListingAsync(Listing listing);
}