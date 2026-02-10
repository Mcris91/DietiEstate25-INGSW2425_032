using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.BookingModels;

namespace DietiEstate.Infrastructure.Extensions;

public static class BookingQueryExtensions
{
    public static IQueryable<Booking> ApplyFilters(this IQueryable<Booking> query, BookingFilterDto filterDto)
    {
        if(filterDto.AgencyId.HasValue)
            query = query.Where(b => b.Listing.Agent.AgencyId == filterDto.AgencyId.Value);
        
        if(filterDto.ListingId.HasValue)
            query = query.Where(b => b.ListingId == filterDto.ListingId);
        
        if(filterDto.AgentId.HasValue)
            query = query.Where(b => b.AgentId == filterDto.AgentId);
        
        if(filterDto.ClientId.HasValue)
            query = query.Where(b => b.ClientId == filterDto.ClientId);
        
        if(filterDto.DateMeeting.HasValue)
            query = query.Where(b => b.DateMeeting >= filterDto.DateMeeting.Value);
        
        if(filterDto.DateMeeting.HasValue)
            query = query.Where(b => b.DateMeeting <= filterDto.DateMeeting.Value); 
        
        if (filterDto.Status.HasValue)
            query = query.Where(b => b.Status == filterDto.Status);

        
        return query;
    }

    public static IQueryable<Booking> ApplySorting(this IQueryable<Booking> query, string sortBy, string sortOrder)
    {
        string sortField = sortBy?.ToLower() ?? "date";

        return sortField switch
        {
            // Gestiamo sia 'date' che 'data' per sicurezza
            "date" or "data" => sortOrder == "desc" ? query.OrderByDescending(b => b.DateMeeting) : query.OrderBy(b => b.DateMeeting),
        
            // IL PEZZO MANCANTE: Il caso di default (_) gestisce qualsiasi altro valore (incluso "data" se non mappato sopra)
            _ => query.OrderByDescending(b => b.Id) 
        };
    }
}