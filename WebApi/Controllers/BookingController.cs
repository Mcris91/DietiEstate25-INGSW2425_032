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
    public async Task<ActionResult<PagedResponseDto<BookingResponseDto>>> GetBookings(
        [FromQuery] BookingFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingsAsync(filterDto,pageNumber,pageSize);
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.Select(mapper.Map<BookingResponseDto>), pageNumber, pageSize));
    }
    
    [HttpGet("{bookingId:guid}")]
   
    public async Task<IActionResult> GetBookingById(Guid bookingId) 
    {
        if (await bookingRepository.GetBookingByIdAsync(bookingId) is { } booking)
            return Ok(mapper.Map<BookingResponseDto>(booking));
        
        return NotFound();
    }

    [HttpGet("GetByListingId/{listingId:guid}")]

    public async Task<ActionResult<PagedResponseDto<BookingResponseDto>>> GetBookingByListingId(
        Guid listingId,
        [FromQuery] BookingFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingByIdListingAsync(listingId, filterDto,pageNumber,pageSize);
        return  Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select(mapper.Map<BookingResponseDto>), pageNumber, pageSize));
    }

    [HttpGet("GetByAgentId/{agentId:guid}")]

    public async Task<ActionResult<PagedResponseDto<BookingResponseDto>>> GetBookingByAgentId(
        Guid agentId,
        [FromQuery] BookingFilterDto filterDto,
        [FromQuery]  int? pageNumber,
        [FromQuery]  int? pageSize)
    {
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingByAgentIdAsync(agentId, filterDto, pageNumber,pageSize);
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select(mapper.Map<BookingResponseDto>), pageNumber, pageSize));
    }

    [HttpGet("GetByClientId/{clientId:guid}")]

    public async Task<ActionResult<PagedResponseDto<BookingResponseDto>>> GetBookingByClientId(
        Guid clientId,
        [FromQuery] BookingFilterDto filterDto,
        [FromQuery]  int? pageNumber,
        [FromQuery]  int? pageSize)
    {
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingByClientIdAsync(clientId, filterDto, pageNumber,pageSize);
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select( mapper.Map<BookingResponseDto>), pageNumber, pageSize));
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