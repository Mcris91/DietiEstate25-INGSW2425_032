using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.BookingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking?>> GetBookingsAsync(BookingFilterDto filterDto);
    Task<Booking?> GetBookingByIdAsync(Guid bookingId);
    
    Task<IEnumerable<Booking?>> GetBookingByIdListingAsync(Guid listingId, BookingFilterDto filterDto);
    
    Task<IEnumerable<Booking?>> GetBookingByAgentIdAsync(Guid agentId, BookingFilterDto filterDto);
    
    Task<IEnumerable<Booking?>> GetBookingByClientIdAsync(Guid clientId, BookingFilterDto filterDto);
    
    Task AddBookingAsync(Booking booking);
    Task UpdateBookingAsync(Booking booking);
    Task DeleteBookingAsync(Booking booking);
}