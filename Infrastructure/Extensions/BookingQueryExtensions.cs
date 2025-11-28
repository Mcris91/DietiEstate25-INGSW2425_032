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
}