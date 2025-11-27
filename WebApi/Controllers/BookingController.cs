using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
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
    public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetBookings(
        [FromQuery] BookingFilterDto filterDto)
    {
        var bookings = await bookingRepository.GetBookingsAsync(filterDto);
        return Ok(bookings.ToList().Select(mapper.Map<BookingResponseDto>));
    }
    
    [HttpGet("{bookingId:guid}")]
   
    public async Task<IActionResult> GetBookingById(Guid bookingId) 
    {
        if (await bookingRepository.GetBookingByIdAsync(bookingId) is { } booking)
            return Ok(mapper.Map<BookingResponseDto>(booking));
        
        return NotFound();
    }

    [HttpGet("GetByListingId/{listingId:guid}")]

    public async Task<ActionResult<BookingResponseDto>> GetBookingByListingId(
        Guid listingId,
        [FromQuery] BookingFilterDto filterDto)
    {
        var bookings = await bookingRepository.GetBookingByIdListingAsync(listingId, filterDto);
        return  Ok(bookings.ToList().Select(mapper.Map<BookingResponseDto>));
    }

    [HttpGet("GetByAgentId/{agentId:guid}")]

    public async Task<ActionResult<BookingResponseDto>> GetBookingByAgentId(
        Guid agentId,
        [FromQuery] BookingFilterDto filterDto)
    {
        var bookings = await bookingRepository.GetBookingByAgentIdAsync(agentId, filterDto);
        return Ok(bookings.ToList().Select(mapper.Map<BookingResponseDto>));
    }

    [HttpGet("GetByClientId/{clientId:guid}")]

    public async Task<ActionResult<BookingResponseDto>> GetBookingByClientId(
        Guid clientId,
        [FromQuery] BookingFilterDto filterDto)
    {
        var bookings = await bookingRepository.GetBookingByClientIdAsync(clientId, filterDto);
        return Ok(bookings.ToList().Select(mapper.Map<BookingResponseDto>));
    }

    [HttpPost]
    public async Task<IActionResult> PostBooking(
        [FromQuery] BookingRequestDto request)
    {
        var booking = mapper.Map<Booking>(request);
        await bookingRepository.AddBookingAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { bookingId = booking.Id }, mapper.Map<BookingResponseDto>(booking));
    }

    [HttpPut("{bookingId:guid}")]
    public async Task<IActionResult> PutBooking(Guid bookingId, [FromBody]  BookingRequestDto request)
    {
        if(await bookingRepository.GetBookingByIdAsync(bookingId) is not { } booking)
            return NotFound();
        
        await bookingRepository.UpdateBookingAsync(booking);
        return Ok(mapper.Map<BookingResponseDto>(booking));
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