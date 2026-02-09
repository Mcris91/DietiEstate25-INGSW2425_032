using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.OfferModels;

namespace DietiEstate.Infrastracture.Extensions;

public static class OfferQueryExtensions
{
    public static IQueryable<Offer> ApplyFilters(this IQueryable<Offer> query, OfferFilterDto filters)
    {
        if (filters.AgencyId.HasValue)
            query = query.Where(o => o.Listing.Agent.AgencyId == filters.AgencyId.Value);
        
        if (filters.AgentId.HasValue)
            query = query.Where(o => o.AgentId == filters.AgentId.Value);
        
        if (!string.IsNullOrEmpty(filters.CustomerFirstName) && !string.IsNullOrEmpty(filters.CustomerLastName))
            query = query.Where(o => o.Customer.FirstName == filters.CustomerFirstName && o.Customer.LastName == filters.CustomerLastName);
        
        if (!string.IsNullOrEmpty(filters.ListingName))
            query = query.Where(o => o.Listing.Name == filters.ListingName);
        
        if (!string.IsNullOrEmpty(filters.Status.ToString()))
            query = query.Where(o => o.Status == filters.Status);
        
        if (!string.IsNullOrEmpty(filters.Date.ToString()))
            query = query.Where(o => o.Date >= filters.Date);
        
        if (filters.Value.HasValue)
            query = query.Where(o => o.Value >= filters.Value);

        return query;
    }
    
    public static IQueryable<Offer> ApplySorting(this IQueryable<Offer> query, string sortBy, string sortOrder)
    {
        return sortBy.ToLower() switch
        {
            "customer" => sortOrder == "desc" ? query.OrderByDescending(o => o.Customer.FirstName) : query.OrderBy(o => o.Customer.FirstName),
            "listing" => sortOrder == "desc" ? query.OrderByDescending(o => o.Listing.Name) : query.OrderBy(o => o.Listing.Name),
            "status" => sortOrder == "desc" ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),
            "value" => sortOrder == "desc" ? query.OrderByDescending(o => o.Value) : query.OrderBy(o => o.Value),
            "date" => sortOrder == "desc" ? query.OrderByDescending(o => o.Date) : query.OrderBy(o => o.Date),
            _ => query.OrderBy(o => o.Status)
        };
    }
}