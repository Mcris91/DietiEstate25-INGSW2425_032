using AutoMapper;
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
    IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<IActionResult> PostOffer([FromBody] OfferRequestDto request)
    {
        var offer = mapper.Map<Offer>(request);
        var listing = await listingRepository.GetListingByIdAsync(offer.ListingId);

        if (listing is null)
            return NotFound();
        
        if (listing is { Available: false }) 
            return BadRequest("L'immobile è già stato venduto");
        
        if (await offerRepository.CheckExistingCustomerOffer(offer.CustomerId)) 
            return BadRequest("Hai già fatto un'offerta per questo immobile");

        if (offer.Value > listing.Price) 
            return BadRequest("Il valore dell'offerta non può essere superiore al valore dell'immobile");

        if (offer.FirstOfferId == Guid.Empty)
            offer.FirstOfferId = offer.Id;
        
        await offerRepository.AddOfferAsync(offer);
        return CreatedAtAction(nameof(GetOfferById), new { offerId = offer.Id }, mapper.Map<OfferResponseDto>(offer));
        
    }

    [HttpGet]
    public async Task<IActionResult> GetOfferById(Guid offerId)
    {
        if (await offerRepository.GetOffersByIdAsync(offerId) is { } offer)
            return Ok(mapper.Map<OfferResponseDto>(offer));
        return NotFound();
    }

    [HttpPut("{offerId:guid}/{accept:bool}")]
    public async Task<IActionResult> AcceptOrRejectOffer(Guid offerId, bool accept)
    {
        var offer = await offerRepository.GetOffersByIdAsync(offerId);
        if (offer is null) return NotFound();
        
        offer.Status = accept ? OfferStatus.Accepted : OfferStatus.Rejected;
        await offerRepository.UpdateOfferAsync(offer);
        return Ok();
    }

    [HttpDelete("{offerId:guid}/{customerId:guid}")]
    public async Task<IActionResult> DeleteOffer(Guid listingId, Guid offerId)
    {
        if (await offerRepository.GetOffersByIdAsync(listingId) is not { } offer) 
            return NotFound();
        
        if (offer.CustomerId != offerId) 
            return BadRequest();
        
        await offerRepository.DeleteOfferAsync(offer);
        return NoContent();
    }
    
}