using AutoMapper;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.BookingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BookingController(
    IBookingRepository bookingRepository,
    IMapper mapper) : Controller
{
    [HttpGet]
    [Authorize(Policy = "ReadBooking")]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await bookingRepository.GetBookingsAsync();
        return Ok(bookings);
    }
    
    [HttpGet("{bookingId:guid}")]
    [Authorize(Policy = "ReadBooking")]
    public async Task<IActionResult> GetBookingById(Guid bookingId) 
    {
        if (await bookingRepository.GetBookingByIdAsync(bookingId) is { } booking)
            return Ok(booking);
        
        return NotFound();
    }

    [HttpPost]
    [Authorize(Policy = "CreateBooking")]
    public async Task<IActionResult> CreateBooking(Booking booking)
    {
        await bookingRepository.AddBookingAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { bookingId = booking.Id }, booking);
    }

    [HttpPatch("{bookingId:guid}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> UpdateBooking(Guid bookingId)
    {
        if(await bookingRepository.GetBookingByIdAsync(bookingId) is not { } booking)
            return NotFound();
        
        await bookingRepository.UpdateBookingAsync(booking);
        return NoContent();
    }

    [HttpDelete("{bookingId:guid}")]
    [Authorize(Roles = "Client")]

    public async Task<IActionResult> DeleteBooking(Guid bookingId)
    {
        if(await bookingRepository.GetBookingByIdAsync(bookingId) is not { } booking)
            return NotFound();
        
        await bookingRepository.DeleteBookingAsync(booking);
        return NoContent();
    }
    
    
}