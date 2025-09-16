using AutoMapper;
using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Dtos.Requests;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.ListingModels;
using DietiEstate.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

/// <summary>
/// A controller responsible for managing listing-related endpoints, including retrieval, creation, updating, and deletion of listings.
/// This controller supports various operations such as paginated data retrieval, modification, and detailed information fetching.
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ListingController(
    IListingRepository listingRepository,
    IMapper mapper) : Controller
{
    /// <summary>
    /// Retrieves a paginated list of listings based on the provided filter criteria.
    /// </summary>
    /// <param name="filterDto">The filter criteria to apply on the listings.</param>
    /// <param name="pageNumber">The page number for the paginated response (must be greater than zero).</param>
    /// <param name="pageSize">The number of listings to include on each page (must be greater than zero).</param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing a paginated response of type <see cref="PagedResponseDto{ListingResponseDto}"/>
    /// with the filtered listings, or a bad request response if the pagination parameters are invalid.
    /// </returns>
    [HttpGet]
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

    /// <summary>
    /// Retrieves a listing by its unique identifier.
    /// </summary>
    /// <param name="listingId">The unique identifier of the listing to retrieve.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the listing in an <see cref="ListingResponseDto"/> format if found,
    /// or a not found response if the listing does not exist.
    /// </returns>
    [HttpGet("{listingId:guid}")]
    public async Task<IActionResult> GetListingById(Guid listingId) 
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is { } listing)
            return Ok(mapper.Map<ListingResponseDto>(listing));
        
        return NotFound();
    }

    /// <summary>
    /// Creates a new listing using the provided request data.
    /// </summary>
    /// <param name="request">The details of the listing to be created, encapsulated in a <see cref="ListingRequestDto"/>.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a created response with the newly added listing details as a <see cref="ListingResponseDto"/>,
    /// including its unique identifier, or an error response if the operation fails.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> PostListing([FromBody] ListingRequestDto request)
    {
        var listing = mapper.Map<Listing>(request);
        await listingRepository.AddListingAsync(listing, request.Services, request.Tags, request.Images);
        return CreatedAtAction(nameof(GetListingById), new {listingId = listing.Id}, mapper.Map<ListingResponseDto>(listing));
    }

    /// <summary>
    /// Applies a JSON Patch document to update an existing listing identified by its unique identifier.
    /// </summary>
    /// <param name="listingId">The unique identifier of the listing to be updated.</param>
    /// <param name="patchDocument">The JSON Patch document containing the changes to be applied to the listing.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation:
    /// - <see cref="NotFoundResult"/> if the listing with the specified identifier does not exist.
    /// - <see cref="BadRequestObjectResult"/> if the patch document is invalid or the model state is not valid after applying the patch.
    /// - <see cref="NoContentResult"/> if the update is successful.
    /// </returns>
    [HttpPatch("{listingId:guid}")]
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

    /// <summary>
    /// Deletes an existing listing identified by its unique identifier.
    /// </summary>
    /// <param name="listingId">The unique identifier of the listing to delete.</param>
    /// <returns>
    /// A <see cref="NoContentResult"/> if the deletion is successful,
    /// a <see cref="NotFoundResult"/> if the listing does not exist.
    /// </returns>
    [HttpDelete("{listingId:guid}")]
    public async Task<IActionResult> DeleteListing(Guid listingId)
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is not { } listing)
            return NotFound();
        await listingRepository.DeleteListingAsync(listing);
        return NoContent();
    }
}
