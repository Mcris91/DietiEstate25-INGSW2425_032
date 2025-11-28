using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.BookingModels;

namespace DietiEstate.Infrastracture.Extensions;

public static class BookingQueryExtensions
{
    public static IQueryable<Booking> ApplyFilters(this IQueryable<Booking> query, BookingFilterDto filterDto)
    {
        
        if(filterDto.ListingId.HasValue)
            query = query.Where(b => b.ListingId == filterDto.ListingId);
        
        if(filterDto.AgentId.HasValue)
            query = query.Where(b => b.AgentUserId == filterDto.AgentId);
        
        if(filterDto.ClientId.HasValue)
            query = query.Where(b => b.ClientUserId == filterDto.ClientId);
        
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
        return sortBy.ToLower() switch
        {
            "DateCreation" => sortOrder == "desc" ? query.OrderByDescending(b => b.DateCreation) : query.OrderBy(b => b.DateCreation),
            "DateMeeting" => sortOrder == "desc" ? query.OrderByDescending(b => b.DateMeeting) : query.OrderBy(b => b.DateMeeting)
        };
    }
}