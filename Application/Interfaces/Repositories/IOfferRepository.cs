using DietiEstate.Core.Entities.OfferModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IOfferRepository
{
    Task AddOfferAsync(Offer offer);
    
    Task UpdateOfferAsync(Offer offer);
    
    Task DeleteOfferAsync(Offer offer);

    Task<Offer?> GetOffersByIdAsync(Guid offerId);

    Task<IEnumerable<Offer>> GetOffersByListingAsync(Guid listingId, int? pageNumber, int? pageSize);

    Task<IEnumerable<Offer>> GetOffersByCustomerAsync(Guid listingId, int? pageNumber, int? pageSize);
    
    Task<IEnumerable<Offer>> GetOffersByAgentAsync(Guid listingId, int? pageNumber, int? pageSize);

    Task<bool> CheckExistingCustomerOffer(Guid userId);

}