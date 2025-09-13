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
    
    [HttpGet("{listingId:guid}")]
    public async Task<IActionResult> GetListingById(Guid listingId) 
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is { } listing)
            return Ok(mapper.Map<ListingResponseDto>(listing));
        
        return NotFound();
    }
}