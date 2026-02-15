using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastructure.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class OfferController(
    IOfferRepository offerRepository,
    IListingRepository listingRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IBackgroundJobClient jobClient,
    //RedisSessionService redisSessionService,
    IMapper mapper) : Controller
{
    //sia customer che agent
    [HttpPost]
    public async Task<IActionResult> PostOffer([FromBody] OfferRequestDto request)
    {
        var offer = mapper.Map<Offer>(request);
        var listing = await listingRepository.GetListingByIdAsync(offer.ListingId);

        if (listing is null)
            return NotFound();
        
        if (listing is { Available: false }) 
            return BadRequest("L'immobile è già stato venduto");
        
        if (string.Equals(User.GetRole(),"Client"))
            if (await offerRepository.CheckExistingCustomerOffer(offer.CustomerId, listing.Id))
                return BadRequest("Hai già un'offerta in sospeso per questo immobile");

        if (offer.Value <= 0 || offer.Value > listing.Price) 
            return BadRequest("Valore dell'offerta non valido");

        if (offer.FirstOfferId == Guid.Empty)
        {
            offer.FirstOfferId = offer.Id;
        }
        else
        {
            var firstOffer = await offerRepository.GetOfferByIdAsync(offer.FirstOfferId);
            
            if (firstOffer is null)
                return BadRequest("Nessuna offerta trovata");
            
            if (firstOffer.Id != firstOffer.FirstOfferId && string.Equals(User.GetRole(), "EstateAgent"))
                return BadRequest("Non puoi inserire più contro offerte per la stessa offerta");

            firstOffer.Status = OfferStatus.Rejected;
        }
        
        offer.Date = offer.Date.UtcDateTime;
        await offerRepository.AddOfferAsync(offer);
        var user = await userRepository.GetUserByIdAsync(offer.AgentId);
        var emailData = await emailService.PrepareNewBookingEmailAsync(user, listing.Name);
        jobClient.Enqueue(() => emailService.SendEmailAsync(emailData));
        return CreatedAtAction(nameof(GetOfferById), new { offerId = offer.Id }, mapper.Map<OfferResponseDto>(offer));
    }

    //sia customer che agent
    [HttpGet]
    public async Task<IActionResult> GetOfferById(Guid offerId)
    {
        if (await offerRepository.GetOfferByIdAsync(offerId) is { } offer)
            return Ok(mapper.Map<OfferResponseDto>(offer));
        return NotFound();
    }

    //agent
    [HttpPut("AcceptOrRejectOffer/{offerId:guid}/{accept:bool}")]
    public async Task<IActionResult> AcceptOrRejectOffer(Guid offerId, bool accept)
    {
        // offerId - CE1: esiste valido, CE2: non esiste non valido, CE3: null non valido
        // accept - CE4: true valido, CE5 false valido // accept non può essere null
        
        // Test1 - CE1, CE4
        // Test2 - CE2, CE5
        
        var offer = await offerRepository.GetOfferByIdAsync(offerId);
        if (offer is null) return NotFound();

        if (offer.Id != offer.FirstOfferId)
            return BadRequest("Non puoi accettare o rifiutare una contro offerta");
        
        if (offer.Status != OfferStatus.Pending)
            return Unauthorized();
        
        offer.Status = accept ? OfferStatus.Accepted : OfferStatus.Rejected;
        
        var listing = await listingRepository.GetListingByIdAsync(offer.ListingId);
        if (listing is null) return NotFound();
        
        if (!listing.Available)
            return Unauthorized();
        
        // Da inserire in una transaction
        await offerRepository.UpdateOfferAsync(offer);
        if (accept)
        {
            var offers = await offerRepository.GetPendingOffersByListingIdAsync(offer.ListingId);
            foreach (var o in offers)
            {
                o.Status = OfferStatus.Rejected;
                await offerRepository.UpdateOfferAsync(o);
            }
            listing.Available = false;
            await listingRepository.UpdateListingAsync(listing, null);
        }
        return Ok();
    }

    
    //client
    [HttpPut("AcceptOrRejectCounterOffer/{counterOfferId:guid}/{accept:bool}")]
    public async Task<IActionResult> AcceptOrRejectCounterOffer(Guid counterOfferId, bool accept)
    {
        var offer = await offerRepository.GetOfferByIdAsync(counterOfferId);
        if (offer is null) return NotFound();

        if (offer.Status != OfferStatus.Pending || offer.CustomerId != User.GetUserId())
            return Unauthorized();
        
        offer.Status = accept ? OfferStatus.Accepted : OfferStatus.Rejected;
        
        var listing = await listingRepository.GetListingByIdAsync(offer.ListingId);
        if (listing is null) return NotFound();
        
        if (!listing.Available)
            return Unauthorized();
        
        // Da inserire in una transaction
        await offerRepository.UpdateOfferAsync(offer);
        if (accept)
        {
            var offers = await offerRepository.GetPendingOffersByListingIdAsync(offer.ListingId);
            foreach (var o in offers)
            {
                o.Status = OfferStatus.Rejected;
                await offerRepository.UpdateOfferAsync(o);
            }
            listing.Available = false;
            await listingRepository.UpdateListingAsync(listing, null);
        }
        return Ok();
    }
    
    //sia customer che agent
    [HttpDelete("{offerId:guid}")]
    public async Task<IActionResult> DeleteOffer(Guid offerId)
    {
        if (await offerRepository.GetOfferByIdAsync(offerId) is not { } offer) 
            return NotFound();
        
        if (offer.CustomerId != User.GetUserId() && offer.AgentId != User.GetUserId()) 
            return Unauthorized();
        
        if (offer.Status == OfferStatus.Accepted)
            return BadRequest("L'offerta è già stata accettata dal nostro agente");
        
        if (offer.Status == OfferStatus.Rejected)
            return BadRequest("L'offerta è già stata rifiutata dal nostro agente");
        
        await offerRepository.DeleteOfferAsync(offer);
        return NoContent();
    }
    
    //agent
    [HttpGet("GetByAgentId")]
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOffersByAgentId(
        [FromQuery] OfferFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
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

        var offers = await offerRepository.GetOffersByAgentIdAsync(filterDto);
        return Ok(new PagedResponseDto<OfferResponseDto>(
            offers.ToList().Select(mapper.Map<OfferResponseDto>), 
            pageSize, pageNumber));
    }
    
    //customer
    [HttpGet("GetByCustomerId")]
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOffersByCustomerId(
        [FromQuery] OfferFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        var customerId = User.GetUserId();
        if (customerId == Guid.Empty)
            return Unauthorized();
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var offers = await offerRepository.GetOffersByCustomerIdAsync(customerId, filterDto);
        return Ok(new PagedResponseDto<OfferResponseDto>(
            offers.ToList().Select(mapper.Map<OfferResponseDto>), 
            pageSize, pageNumber));
    }
    
    //sia customer che agent
    [HttpGet("GetOfferHistory/{offerId:guid}")]
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOfferHistory(
        Guid offerId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        if (await offerRepository.GetOfferByIdAsync(offerId) is not { } offer) 
            return NotFound();
        
        var offers = await offerRepository.GetOfferHistoryAsync(offer.FirstOfferId);
        return Ok(new PagedResponseDto<OfferResponseDto>(
            offers.ToList().Select(mapper.Map<OfferResponseDto>), 
            pageSize, pageNumber));
    }

    [HttpGet("GetTotalOffers")]
    public async Task<ActionResult> GetTotalOffers()
    {
        var agentId = User.GetUserId();
        if (agentId == Guid.Empty)
            return Unauthorized();
        
        OfferFilterDto filters = new();
        
        try
        {
            filters.ApplyRoleFilters(User.GetRole(), User.GetUserId(), User.GetAgencyId());
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        
        var offers = await offerRepository.GetTotalOffersAsync(filters);
        return Ok(new OfferAgentCountersResponseDto()
        {
            TotalOffers = offers.Total,
            PendingOffers = offers.Pending
        });
    }
}