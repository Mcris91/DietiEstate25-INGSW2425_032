using AutoMapper;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.FavouritesModels;
using DietiEstate.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FavouritesController(
    IMapper mapper,
    IFavouritesRepository favouritesRepository) : Controller
{
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<PagedResponseDto<UserFavouritesResponseDto>>> GetFavourites(
        [FromRoute] Guid userId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var favourites = await favouritesRepository.GetFavouritesAsync(userId);
        var favouriteListings = favourites.Select(f => f.Listing).ToList();
        
        return Ok(new PagedResponseDto<UserFavouritesResponseDto>(
            favouriteListings.ToList().Select(mapper.Map<UserFavouritesResponseDto>), 
            pageSize, pageNumber));
    }
    
    [HttpGet("get-user-favourite/{listingId:guid}")]
    public async Task<ActionResult<bool>> GetFavouriteByListingId(
        [FromRoute] Guid listingId)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        var hasFavourite = await favouritesRepository.GetFavouriteAsync(userId, listingId);
        return Ok(hasFavourite != null);
    }

    [HttpPost("{listingId:guid}")]
    public async Task<ActionResult> CreateFavouriteAsync([FromRoute] Guid listingId)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        var favourite = new Favourite()
        {
            ListingId = listingId,
            UserId = userId
        };
        await favouritesRepository.CreateFavouriteAsync(favourite);
        return Ok();
    }
    
    
    [HttpDelete("{listingId:guid}")]
    public async Task<ActionResult> DeleteFavouriteAsync([FromRoute] Guid listingId)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        if (await favouritesRepository.GetFavouriteAsync(userId, listingId) is not { } favourite)
            return NotFound();
        await favouritesRepository.DeleteFavouriteAsync(favourite);
        return NoContent();
    }
}

















