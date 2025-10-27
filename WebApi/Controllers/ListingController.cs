using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ListingController(
    IListingRepository listingRepository,
    IMapper mapper) : Controller
{
    [HttpGet]
    [Authorize(Policy = "ReadListing")]
    public async Task<ActionResult<PagedResponseDto<ListingResponseDto>>> GetListings(
        [FromQuery] ListingFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var listings = await listingRepository.GetListingsAsync(filterDto, pageNumber, pageSize);
        return Ok(new PagedResponseDto<ListingResponseDto>(
            listings.ToList().Select(mapper.Map<ListingResponseDto>), 
            pageSize, pageNumber));
    }
    
    [HttpGet("{listingId:guid}")]
    [Authorize(Policy = "ReadListing")]
    public async Task<IActionResult> GetListingById(Guid listingId) 
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is { } listing)
            return Ok(mapper.Map<ListingResponseDto>(listing));
        
        return NotFound();
    }

    [HttpGet("GetByAgentId/{agentId:guid}")]
    [Authorize(Policy = "ReadListing")]
    public async Task<ActionResult<PagedResponseDto<ListingResponseDto>>> GetListingsByAgentId(
        Guid agentId,
        [FromQuery] ListingFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var listings = await listingRepository.GetListingsByAgentIdAsync(agentId, filterDto, pageNumber, pageSize);
        return Ok(new PagedResponseDto<ListingResponseDto>(
            listings.ToList().Select(mapper.Map<ListingResponseDto>), 
            pageSize, pageNumber));
    }

    [HttpPost]
    [Authorize(Policy = "WriteListing")]
    public async Task<IActionResult> PostListing([FromBody] ListingRequestDto request)
    {
        var listing = mapper.Map<Listing>(request);
        await listingRepository.AddListingAsync(listing, request.Services, request.Tags, request.Images);
        return CreatedAtAction(nameof(GetListingById), new {listingId = listing.Id}, mapper.Map<ListingResponseDto>(listing));
    }

    [HttpPatch("{listingId:guid}")]
    [Authorize(Roles = "SupportAdminOnly")]
    public async Task<IActionResult> PatchListing(Guid listingId, [FromBody] JsonPatchDocument<ListingRequestDto> patchDocument)
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is not { } listing)
            return NotFound();
        
        var listingToPatch = mapper.Map<ListingRequestDto>(listing);
        patchDocument.ApplyTo(listingToPatch, ModelState);
        if (!TryValidateModel(listingToPatch))
            return BadRequest(ModelState);
        
        mapper.Map(listingToPatch, listing);
        await listingRepository.UpdateListingAsync(listing);
        return NoContent();
    }

    [HttpDelete("{listingId:guid}")]
    [Authorize(Roles = "SupportAdminOnly")]
    public async Task<IActionResult> DeleteListing(Guid listingId)
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is not { } listing)
            return NotFound();
        await listingRepository.DeleteListingAsync(listing);
        return NoContent();
    }
}
