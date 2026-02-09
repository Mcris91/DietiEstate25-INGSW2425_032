using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.BookingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking?>> GetBookingsAsync(BookingFilterDto filterDto, int? pageNumber, int? pageSize);
    Task<Booking?> GetBookingByIdAsync(Guid bookingId);
    
    Task<IEnumerable<Booking?>> GetBookingByIdListingAsync(Guid listingId, BookingFilterDto filterDto, int? pageNumber, int? pageSize);
    
    Task<IEnumerable<Booking?>> GetBookingByAgentIdAsync(BookingFilterDto filterDto);
    
    Task<IEnumerable<Booking?>> GetBookingByClientIdAsync(Guid clientId, BookingFilterDto filterDto, int? pageNumber, int? pageSize);
    
    Task<(int Total, int Pending)> GetTotalBookingsAsync(BookingFilterDto filters);
    Task AddBookingAsync(Booking booking);
    Task UpdateBookingAsync(Booking booking);
    Task DeleteBookingAsync(Booking booking);
}