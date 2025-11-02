using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Repositories;

public class OfferRepository(DietiEstateDbContext context) : IOfferRepository
{
    public async Task AddOfferAsync(Offer offer)
    {
        await context.Database.BeginTransactionAsync();
        await context.Offer.AddAsync(offer);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task UpdateOfferAsync(Offer offer)
    {
        await context.Database.BeginTransactionAsync();
        context.Offer.Update(offer);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
    
    public async Task DeleteOfferAsync(Offer offer)
    {
        await context.Database.BeginTransactionAsync();
        context.Offer.Remove(offer);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
    
    public async Task<Offer?> GetOffersByIdAsync(Guid offerId)
    {
        return await context.Offer
            .Where(o => o.Id == offerId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Offer>> GetOffersByListingAsync(Guid listingId, int? pageNumber, int? pageSize)
    {
        return await context.Offer
            .Where(o => o.ListingId == listingId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Offer>> GetOffersByCustomerAsync(Guid customerId, int? pageNumber, int? pageSize)
    {
        return await context.Offer
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Offer>> GetOffersByAgentAsync(Guid agentId, int? pageNumber, int? pageSize)
    {
        return await context.Offer
            .Where(o => o.AgentId == agentId)
            .ToListAsync();
    }

    public async Task<bool> CheckExistingCustomerOffer(Guid userId)
    {
        return await context.Offer
            .Where(o => o.CustomerId == userId)
            .AnyAsync();
    }
}