using DietiEstate.Core.Entities.BookingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking?>> GetBookingsAsync();
    Task<Booking?> GetBookingByIdAsync(Guid bookingId);
    
    Task AddBookingAsync(Booking booking);
}