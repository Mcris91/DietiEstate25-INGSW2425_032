using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Repositories;

public class BookingRepository(DietiEstateDbContext context) : IBookingRepository
{
    public async Task<IEnumerable<Booking?>> GetBookingsAsync()
    {
        return await context.Booking.ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
    {
        return await context.Booking.FindAsync(bookingId);
    }

    public async Task AddBookingAsync(Booking booking)
    {
        await context.Booking.AddAsync(booking);
    }

}