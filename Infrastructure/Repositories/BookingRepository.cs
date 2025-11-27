using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Repositories;

public class BookingRepository(DietiEstateDbContext context) : IBookingRepository
{
    public async Task<IEnumerable<Booking?>> GetBookingsAsync(BookingFilterDto filterDto)
    {
        return await context.Booking.ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
    {
        return await context.Booking.FindAsync(bookingId);
    }

    public async Task<IEnumerable<Booking?>> GetBookingByIdListingAsync(Guid listingId, BookingFilterDto filterDto)
    {
        return await context.Booking.ToListAsync();
    }

    public async Task<IEnumerable<Booking?>> GetBookingByAgentIdAsync(Guid agentId, BookingFilterDto filterDto)
    {
        return await context.Booking.ToListAsync();
    }

    public async Task<IEnumerable<Booking?>> GetBookingByClientIdAsync(Guid clientId, BookingFilterDto filterDto)
    {
        return await context.Booking.ToListAsync();
    }

    public async Task AddBookingAsync(Booking booking)
    {
        await context.Database.BeginTransactionAsync();
        await context.Booking.AddAsync(booking);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        await context.Database.BeginTransactionAsync();
        context.Booking.Update(booking);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }

    public async Task DeleteBookingAsync(Booking booking)
    {
        await context.Database.BeginTransactionAsync();
        context.Booking.Remove(booking);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}