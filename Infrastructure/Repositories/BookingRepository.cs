using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Infrastracture.Data;

namespace DietiEstate.Infrastracture.Repositories;

public class BookingRepository(DietiEstateDbContext context) : IBookingRepository
{
    public async Task<IEnumerable<Booking?>> GetBookingsAsync()
    {
        return await context.Booking;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
    {
        return await context.Booking;
    }

}