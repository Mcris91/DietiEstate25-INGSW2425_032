using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;
using DietiEstate.Infrastracture.Extensions;

namespace DietiEstate.Infrastracture.Repositories;

public class BookingRepository(DietiEstateDbContext context) : IBookingRepository
{
    public async Task<IEnumerable<Booking?>> GetBookingsAsync(BookingFilterDto filterDto, int? pageNumber, int? pageSize)
    {
        return await context.Booking
            .Include(b => b.Listing)
            .Include(b => b.Client)
            .ApplyFilters(filterDto)
            .ApplySorting(filterDto.SortBy, filterDto.SortOrder)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
    {
        return await context.Booking.FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    public async Task<IEnumerable<Booking?>> GetBookingByIdListingAsync(Guid listingId, BookingFilterDto filterDto,int? pageNumber, int? pageSize)
    {
        return await context.Booking
            .ApplyFilters(filterDto)
            .ApplySorting(filterDto.SortBy, filterDto.SortOrder)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking?>> GetBookingByAgentIdAsync(Guid agentId, BookingFilterDto filterDto, int? pageNumber, int? pageSize)
    {
        return await context.Booking
            .Include(b => b.Listing)
            .Include(b => b.Client)
            .ApplyFilters(filterDto)
            .ApplySorting(filterDto.SortBy, filterDto.SortOrder)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking?>> GetBookingByClientIdAsync(Guid clientId, BookingFilterDto filterDto, int? pageNumber, int? pageSize)
    {
        return await context.Booking
            .ApplyFilters(filterDto)
            .ApplySorting(filterDto.SortBy, filterDto.SortOrder)
            .ToListAsync();
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
    
    public async Task<(int Total, int Pending)> GetTotalBookingsAsync(Guid agentId)
    {
        var totalBookings = context.Booking
            .Where(b => b.AgentId == agentId);
        var pendingBookings = totalBookings.Where(b => b.Status == BookingStatus.Pending);
        var scheduledBookings = totalBookings.Where(b => b.Status == BookingStatus.Accepted && b.DateMeeting.ToLocalTime() >= DateTime.Now);
        return (await scheduledBookings.CountAsync(), await pendingBookings.CountAsync());
    }
}