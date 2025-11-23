using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Responses;
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
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await bookingRepository.GetBookingsAsync();
        return Ok(bookings);
    }
    
    [HttpGet("{bookingId:guid}")]
   
    public async Task<IActionResult> GetBookingById(Guid bookingId) 
    {
        if (await bookingRepository.GetBookingByIdAsync(bookingId) is { } booking)
            return Ok(booking);
        
        return NotFound();
    }

    [HttpGet("GetByListingId/{listingId:guid}")]

    public async Task<IActionResult> GetBookingByListingId(Guid listingId)
    {
        var boockings = await bookingRepository.GetBookingByIdListingAsync(listingId);
        return  Ok(boockings);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(Booking booking)
    {
        await bookingRepository.AddBookingAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { bookingId = booking.Id }, booking);
    }

    [HttpPatch("{bookingId:guid}")]
    public async Task<IActionResult> UpdateBooking(Guid bookingId)
    {
        if(await bookingRepository.GetBookingByIdAsync(bookingId) is not { } booking)
            return NotFound();
        
        await bookingRepository.UpdateBookingAsync(booking);
        return NoContent();
    }

    [HttpDelete("{bookingId:guid}")]

    public async Task<IActionResult> DeleteBooking(Guid bookingId)
    {
        if(await bookingRepository.GetBookingByIdAsync(bookingId) is not { } booking)
            return NotFound();
        
        await bookingRepository.DeleteBookingAsync(booking);
        return NoContent();
    }
    
    
}