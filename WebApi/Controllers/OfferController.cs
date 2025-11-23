using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class OfferController(
    IOfferRepository offerRepository,
    IListingRepository listingRepository,
    IUserRepository userRepository,
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
            if (await offerRepository.CheckExistingCustomerOffer(offer.CustomerId))
                return BadRequest("Hai già fatto un'offerta per questo immobile");

        if (offer.Value > listing.Price) 
            return BadRequest("Il valore dell'offerta non può essere superiore al valore dell'immobile");

        if (offer.FirstOfferId == Guid.Empty)
            offer.FirstOfferId = offer.Id;
        
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
            await listingRepository.UpdateListingAsync(listing);
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
    [HttpGet("GetByAgentId/{agentId:guid}")]
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOffersByAgentId(
        Guid agentId,
        [FromQuery] OfferFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var offers = await offerRepository.GetOffersByAgentIdAsync(agentId, filterDto);
        return Ok(new PagedResponseDto<OfferResponseDto>(
            offers.ToList().Select(mapper.Map<OfferResponseDto>), 
            pageSize, pageNumber));
    }
    
    //customer
    [HttpGet("GetByCustomerId/{customerId:guid}")]
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOffersByCustomerId(
        Guid customerId,
        [FromQuery] OfferFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
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

    [HttpGet("GetTotalOffers/{agentId:guid}")]
    public async Task<ActionResult<(int Total, int Pending)>> GetTotalOffers(Guid agentId)
    {
        return await offerRepository.GetTotalOffersAsync(agentId);
    }
}