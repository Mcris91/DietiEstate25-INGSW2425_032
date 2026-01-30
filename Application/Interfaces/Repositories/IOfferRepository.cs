using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.OfferModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IOfferRepository
{
    Task AddOfferAsync(Offer offer);
    
    Task UpdateOfferAsync(Offer offer);
    
    Task DeleteOfferAsync(Offer offer);

    Task<Offer?> GetOfferByIdAsync(Guid offerId);

    Task<IEnumerable<Offer>> GetOffersByCustomerIdAsync(Guid customerId, OfferFilterDto filters);
    
    Task<IEnumerable<Offer?>> GetOffersByAgentIdAsync(Guid agentId, OfferFilterDto filters);

    Task<bool> CheckExistingCustomerOffer(Guid userId);

    Task<IEnumerable<Offer?>> GetPendingOffersByListingIdAsync(Guid listingId);
    Task<IEnumerable<Offer?>> GetOfferHistoryAsync(Guid offerId);
    Task<(int Total, int Pending)> GetTotalOffersAsync(Guid agentId);
}