using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class OfferController(
    IOfferRepository offerRepository,
    IListingRepository listingRepository,
    IUserRepository userRepository,
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
        
        if (await userRepository.GetUserByIdAsync(offer.CustomerId) is { Role: UserRole.Client })
            if (await offerRepository.CheckExistingCustomerOffer(offer.CustomerId, listing.Id))
                return BadRequest("Hai già un'offerta in sospeso per questo immobile");

        if (offer.Value <= 0 || offer.Value > listing.Price) 
            return BadRequest("Valore dell'offerta non valido");

        if (offer.FirstOfferId == Guid.Empty)
            offer.FirstOfferId = offer.Id;

        offer.Date = offer.Date.UtcDateTime;
        
        await offerRepository.AddOfferAsync(offer);
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

    //sia customer che agent
    [HttpDelete("{offerId:guid}/{customerId:guid}")]
    public async Task<IActionResult> DeleteOffer(Guid offerId, Guid customerId)
    {
        if (await offerRepository.GetOfferByIdAsync(offerId) is not { } offer) 
            return NotFound();
        
        if (offer.CustomerId != customerId && offer.AgentId != customerId) 
            return Unauthorized();
        
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
        
        var userRole = User.FindFirst("role")?.Value;
        
        OfferFilterDto filters = new();
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
        
        var offers = await offerRepository.GetTotalOffersAsync(filters);
        return Ok(new OfferAgentCountersResponseDto()
        {
            TotalOffers = offers.Total,
            PendingOffers = offers.Pending
        });
    }
}