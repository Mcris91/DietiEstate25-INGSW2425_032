using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastracture.Data;
using DietiEstate.Infrastracture.Extensions;
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
    
    public async Task<Offer?> GetOfferByIdAsync(Guid offerId)
    {
        return await context.Offer
            .Where(o => o.Id == offerId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Offer>> GetOffersByCustomerIdAsync(Guid customerId, OfferFilterDto filters)
    {
        return await context.Offer
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Listing)
            .ApplyFilters(filters)
            .ApplySorting(filters.SortBy, filters.SortOrder)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Offer?>> GetOffersByAgentIdAsync(OfferFilterDto filters)
    {
        return await context.Offer
            .Include(o => o.Listing)
            .Include(o => o.Customer)
            .ApplyFilters(filters)
            .ApplySorting(filters.SortBy, filters.SortOrder)
            .ToListAsync();
    }

    public async Task<bool> CheckExistingCustomerOffer(Guid userId, Guid listingId)
    {
        return await context.Offer
            .Where(o => o.CustomerId == userId && o.ListingId == listingId)
            .AnyAsync();
    }

    public async Task<IEnumerable<Offer?>> GetPendingOffersByListingIdAsync(Guid listingId)
    {
        return await context.Offer
            .Where(o => o.ListingId == listingId && o.Status == OfferStatus.Pending)
            .ToListAsync();
    }

    public async Task<IEnumerable<Offer?>> GetOfferHistoryAsync(Guid offerId)
    {
        return await context.Offer
            .Where(o => o.FirstOfferId == offerId)
            .OrderBy(o => o.Date)
            .ToListAsync();
    }
    public async Task<(int Total, int Pending)> GetTotalOffersAsync(OfferFilterDto filters)
    {
        var totalOffers = context.Offer.ApplyFilters(filters);
        var pendingOffers = totalOffers.Where(o => o.Status == OfferStatus.Pending);
        return (await totalOffers.CountAsync(), await pendingOffers.CountAsync());
    }
}