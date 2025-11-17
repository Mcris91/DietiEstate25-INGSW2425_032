using AutoMapper;
using DietiEstate.Application.Interfaces.Repositories;
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
            return Ok(mapper.Map(booking));
        
        return NotFound();
    }
    
    
}