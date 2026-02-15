using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastructure.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BookingController(
    IBookingRepository bookingRepository,
    IListingRepository listingRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IBackgroundJobClient jobClient,
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

        try
        {
            filterDto.ApplyRoleFilters(User.GetRole(), User.GetUserId(), User.GetAgencyId());
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingByAgentIdAsync(filterDto);
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select(mapper.Map<BookingResponseDto>), pageSize, pageNumber));
    }

    [HttpGet("GetByClientId")]
    public async Task<ActionResult<PagedResponseDto<BookingResponseDto>>> GetBookingByClientId(
        [FromQuery] BookingFilterDto filterDto,
        [FromQuery]  int? pageNumber,
        [FromQuery]  int? pageSize)
    {
        var clientId = User.GetUserId();
        if (clientId == Guid.Empty)
            return Unauthorized();
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});
        
        var bookings = await bookingRepository.GetBookingByClientIdAsync(clientId, filterDto, pageNumber,pageSize);
        return Ok(new PagedResponseDto<BookingResponseDto>(bookings.ToList().Select( mapper.Map<BookingResponseDto>), pageSize, pageNumber));
    }

    [HttpPost]
    public async Task<IActionResult> PostBooking(
        [FromBody] BookingRequestDto request)
    {
        var booking = mapper.Map<Booking>(request);
        booking.DateMeeting = booking.DateMeeting.ToUniversalTime();
        booking.DateCreation = booking.DateCreation.ToUniversalTime();
        await bookingRepository.AddBookingAsync(booking);
        var user = await userRepository.GetUserByIdAsync(booking.AgentId);
        var listing = await listingRepository.GetListingByIdAsync(booking.ListingId);
        var emailData = await emailService.PrepareNewBookingEmailAsync(user, listing.Name);
        jobClient.Enqueue(() => emailService.SendEmailAsync(emailData));
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
        
        if (booking.ClientId != User.GetUserId() && booking.AgentId != User.GetUserId()) 
            return Unauthorized();

        if (booking.Status == BookingStatus.Accepted)
            return BadRequest("La prenotazione è già stata accettata dal nostro agente");
        
        if (booking.Status == BookingStatus.Rejected)
            return BadRequest("La prenotazione è già stata rifiutata dal nostro agente");
        
        await bookingRepository.DeleteBookingAsync(booking);
        return NoContent();
    }
    
    [HttpGet("GetTotalBookings")]
    public async Task<ActionResult> GetTotalBookings()
    {
        var agentId = User.GetUserId();
        if (agentId == Guid.Empty)
            return Unauthorized();
        
        BookingFilterDto filters = new();
        
        try
        {
            filters.ApplyRoleFilters(User.GetRole(), User.GetUserId(), User.GetAgencyId());
        }
        catch (UnauthorizedAccessException)
        {
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