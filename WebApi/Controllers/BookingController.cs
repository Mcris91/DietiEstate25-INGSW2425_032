using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastracture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BookingController(
    IBookingRepository bookingRepository,
    IListingRepository listingRepository,
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
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select(mapper.Map<BookingResponseDto>), pageSize, pageNumber));
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

    [HttpGet("GetByAgentId")]
    public async Task<ActionResult<PagedResponseDto<BookingResponseDto>>> GetBookingByAgentId(
        [FromQuery] BookingFilterDto filterDto,
        [FromQuery]  int? pageNumber,
        [FromQuery]  int? pageSize)
    {
        var userId = User.GetUserId();
        
        if (userId == Guid.Empty)
            return Unauthorized();
        
        var userRole = User.FindFirst("role")?.Value;
        
        switch (userRole)
        {
            case "EstateAgent":
                filterDto.AgentId = userId;
                filterDto.AgencyId = null;
                break;
            case "SuperAdmin":
            case "SupportAdmin":
            {
                var agencyId = User.GetAgencyId();
                if (agencyId == Guid.Empty)
                    return Unauthorized();
                
                filterDto.AgentId = null;
                filterDto.AgencyId = agencyId;
                break;
            }
            case "SystemAdmin":
                filterDto.AgentId = null;
                filterDto.AgencyId = null;
                break;
            default:
                return Unauthorized();
        }
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingByAgentIdAsync(filterDto);
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select(mapper.Map<BookingResponseDto>), pageSize, pageNumber));
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
        [FromBody] BookingRequestDto request)
    {
        var booking = mapper.Map<Booking>(request);
        booking.DateMeeting = booking.DateMeeting.ToUniversalTime();
        booking.DateCreation = booking.DateCreation.ToUniversalTime();
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
    
    [HttpGet("GetTotalBookings")]
    public async Task<ActionResult> GetTotalBookings()
    {
        var agentId = User.GetUserId();
        if (agentId == Guid.Empty)
            return Unauthorized();
        
        var userRole = User.FindFirst("role")?.Value;
        
        BookingFilterDto filters = new();
        switch (userRole)
        {
            case "EstateAgent":
                filters.AgentId = agentId;
                filters.AgencyId = null;
                break;
            case "SuperAdmin":
            case "SupportAdmin":
            {
                var agencyId = User.GetAgencyId();
                if (agencyId == Guid.Empty)
                    return Unauthorized();
                
                filters.AgentId = null;
                filters.AgencyId  = agencyId;
                break;
            }
            case "SystemAdmin":
                filters.AgentId = null;
                filters.AgencyId = null;
                break;
            default:
                return Unauthorized();
        }
        
        var bookings = await bookingRepository.GetTotalBookingsAsync(filters);
        return Ok(new BookingAgentCountersResponseDto()
        {
            ScheduledBookings = bookings.Total,
            PendingBookings = bookings.Pending
        });
    }
    
    [HttpPut("AcceptOrRejectBooking/{bookingId:guid}/{accept:bool}")]
    public async Task<IActionResult> AcceptOrRejectOffer(Guid bookingId, bool accept)
    {
        var booking = await bookingRepository.GetBookingByIdAsync(bookingId);
        if (booking is null) return NotFound();

        if (booking.Status != BookingStatus.Pending)
            return Unauthorized();
        
        booking.Status = accept ? BookingStatus.Accepted : BookingStatus.Rejected;
        
        var listing = await listingRepository.GetListingByIdAsync(booking.ListingId);
        if (listing is null) return NotFound();
        
        if (!listing.Available)
            return Unauthorized();
        
        await bookingRepository.UpdateBookingAsync(booking);
        
        return Ok();
    }
    
}