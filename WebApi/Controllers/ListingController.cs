using AutoMapper;
using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
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
}