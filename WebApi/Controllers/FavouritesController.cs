using AutoMapper;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.FavouritesModels;
using DietiEstate.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FavouritesController(
    IMapper mapper,
    IMinioService minioService,
    IFavouritesRepository favouritesRepository) : Controller
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<UserFavouritesResponseDto>>> GetFavourites(
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return Unauthorized();
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var favourites = await favouritesRepository.GetFavouritesAsync(userId);
        var favouriteListings = favourites.Select(f => f.Listing).ToList();
        
        foreach (var listing in favouriteListings)
        {
            if (!string.IsNullOrEmpty(listing.FeaturedImage))
                listing.FeaturedImage = await minioService.GeneratePresignedUrl(listing.FeaturedImage);
        }
        
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
    public async Task<ActionResult> CreateFavourite([FromRoute] Guid listingId)
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
    public async Task<ActionResult> DeleteFavourite([FromRoute] Guid listingId)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        if (await favouritesRepository.GetFavouriteAsync(userId, listingId) is not { } favourite)
            return NotFound();
        await favouritesRepository.DeleteFavouriteAsync(favourite);
        return NoContent();
    }

    [HttpGet("favourites-list")]
    public async Task<IActionResult> GetFavouritesIdList()
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        var favourites = await favouritesRepository.GetFavouritesAsync(userId);
        var favouritesId = favourites.Select(f => f.ListingId);
        return Ok(favouritesId.ToList());
    }
}

















