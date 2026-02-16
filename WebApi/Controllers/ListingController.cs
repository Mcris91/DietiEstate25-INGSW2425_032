using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.Common;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastructure.Extensions;
using DietiEstate.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ListingController(
    IListingRepository listingRepository,
    IPropertyTypeRepository propertyTypeRepository,
    IMinioService minioService,
    GeoapifyService geoapifyService,
    IExcelService excelService,
    IMapper mapper) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResponseDto<ListingResponseDto>>> GetListings(
        [FromQuery] ListingFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var listings = await listingRepository.GetListingsAsync(filterDto);
        foreach (var listing in listings)
        {
            if (!string.IsNullOrEmpty(listing.FeaturedImage))
                listing.FeaturedImage = await minioService.GeneratePresignedUrl(listing.FeaturedImage);
        }
        return Ok(new PagedResponseDto<ListingResponseDto>(
            listings.ToList().Select(mapper.Map<ListingResponseDto>), 
            pageSize, pageNumber));
    }
    
    [HttpGet("Dashboard")]
    [Authorize(Policy = "ReadListing")]
    public async Task<ActionResult<PagedResponseDto<ListingResponseDto>>> GetListingsByAgent(
        [FromQuery] ListingFilterDto filterDto,
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

        var listings = await listingRepository.GetDetailedListingsAsync(filterDto);
        
        foreach (var listing in listings)
        {
            if (!string.IsNullOrEmpty(listing.FeaturedImage))
                listing.FeaturedImage = await minioService.GeneratePresignedUrl(listing.FeaturedImage);
        }
        return Ok(new PagedResponseDto<ListingResponseDto>(
            listings.ToList().Select(mapper.Map<ListingResponseDto>), 
            pageSize, pageNumber));
    }

    [HttpGet("RecentListings")]
    [Authorize(Policy = "ReadListing")]
    public async Task<IActionResult> GetRecentListings([FromQuery] List<Guid> listingIdsList,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        var listings = await listingRepository.GetRecentListingsAsync(listingIdsList);
        foreach (var listing in listings)
        {
            if (!string.IsNullOrEmpty(listing.FeaturedImage))
                listing.FeaturedImage = await minioService.GeneratePresignedUrl(listing.FeaturedImage);
        }
        return Ok(new PagedResponseDto<ListingResponseDto>(
            listings.ToList().Select(mapper.Map<ListingResponseDto>), 
            pageSize, pageNumber));
    }

    [HttpGet("{listingId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetListingById(Guid listingId) 
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is { } listing)
        {
            if(!string.IsNullOrEmpty(listing.FeaturedImage))
                listing.FeaturedImage = await minioService.GeneratePresignedUrl(listing.FeaturedImage);
            
            if (listing.ListingImages.Count > 0)
            {
                foreach (var image in listing.ListingImages)
                    image.Url = await minioService.GeneratePresignedUrl(image.Url);
            }
            return Ok(mapper.Map<ListingResponseDto>(listing));
        }
        
        return NotFound();
    }

    [HttpGet("GetAgentCounters")]
    [Authorize(Policy = "ReadListing")]
    public async Task<IActionResult> GetAgentCounters()
    {
        var agentId = User.GetUserId();
        
        if (agentId == Guid.Empty)
            return Unauthorized();
        
        ListingFilterDto filters = new();
        
        try
        {
            filters.ApplyRoleFilters(User.GetRole(), User.GetUserId(), User.GetAgencyId());
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        
        var listings = await listingRepository.GetDetailedListingsAsync(filters);

        var forRentType = await propertyTypeRepository.GetPropertyTypeByCodeAsync("RENT");
        var forSaleType = await propertyTypeRepository.GetPropertyTypeByCodeAsync("SALE");
        
        if (forRentType == null || forSaleType == null)
            return StatusCode(500, new { error = "Property types 'RENT' and 'SALE' must exist in the database." });

        var listingsEnumerable = listings.ToList();
        
        return Ok(new ListingAgentCountersResponseDto()
        {
            ForRentCount = listingsEnumerable.Count(l => l.TypeId == forRentType.Id),
            ForSaleCount = listingsEnumerable.Count(l => l.TypeId == forSaleType.Id)
        });
    }

    [HttpPost]
    [Authorize(Policy = "WriteListing")]
    public async Task<IActionResult> PostListing([FromBody] ListingRequestDto request)
    {
        var listing = mapper.Map<Listing>(request);
        listing.AgentUserId = User.GetUserId();

        if (request.FeaturedImage.Length > 0)
        {
            try
            {
                using var featuredImageStream = new MemoryStream(request.FeaturedImage);
                listing.FeaturedImage = await minioService.UploadImageAsync(featuredImageStream, listing.Id, listing.Id);
            }
            catch (Exception)
            {
                return BadRequest("L'immagine di copertina non è stata caricata");

            }
        }
        foreach (var listingImage in request.Images)
        {
            using var imageStream = new MemoryStream(listingImage.Image);

            var newImage = new Image()
            {
                Id = Guid.NewGuid(),
            };
            try
            {
                newImage.Url = await minioService.UploadImageAsync(imageStream, listing.Id, newImage.Id);
            }
            catch (Exception)
            {
                return BadRequest("L'immagine non è stata caricata");
            }
            listing.ListingImages.Add(newImage);
        }

        listing.ListingServices = await geoapifyService.GetNearbyServicesAsync(listing.Id, listing.Location.Y, listing.Location.X);
        var tags = listing.ListingServices.Select(s => s.Type).Distinct().ToList();
        var type = await propertyTypeRepository.GetPropertyTypeByCodeAsync(request.TypeCode);
        listing.TypeId = type.Id;
        await listingRepository.AddListingAsync(listing, tags);
        return CreatedAtAction(nameof(GetListingById), new {listingId = listing.Id}, mapper.Map<ListingResponseDto>(listing));
    }

    [HttpPatch("{listingId:guid}")]
    [Authorize(Policy = "WriteListing")]
    public async Task<IActionResult> PatchListing(Guid listingId, [FromBody] ListingRequestDto listingDto)
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is not { } listing)
            return NotFound();
        
        if (!TryValidateModel(listingDto))
            return BadRequest(ModelState);

        if (listing.AgentUserId != User.GetUserId())
            return Unauthorized();

        var oldListingAddress = listing.Address;
        var oldFeatuerdImageUrl = listing.FeaturedImage;
        
        
        mapper.Map(listingDto, listing);
        listing.Id = listingId;
        listing.AgentUserId = User.GetUserId();

        listing.ListingImages = listing.ListingImages.Where(i => listingDto.Images.Select(image => image.Id).Contains(i.Id)).ToList();
        
        
        if (listingDto.FeaturedImage.Length > 0)
        {
            try
            {
                using var featuredImageStream = new MemoryStream(listingDto.FeaturedImage);
                listing.FeaturedImage = await minioService.UploadImageAsync(featuredImageStream, listing.Id, listing.Id);
            }
            catch (Exception)
            {
                return BadRequest("L'immagine di copertina non è stata caricata");
            }
        }
        else
        {
            listing.FeaturedImage = oldFeatuerdImageUrl;
        }
        
        foreach (var image in listingDto.Images)
        {
            if (image.Image.Length > 0)
            {
                using var imageStream = new MemoryStream(image.Image);
                var newImage = new Image();
                try
                {
                    newImage.Url = await minioService.UploadImageAsync(imageStream, listing.Id, Guid.NewGuid());
                }
                catch (Exception)
                {
                    return BadRequest("L'immagine non è stata caricata");
                }
                listing.ListingImages.Add(newImage);
            }
            
            
        }
        
        if (!string.Equals(oldListingAddress, listing.Address))
        {
            listing.ListingServices.Clear();
            
            var newServices = await geoapifyService.GetNearbyServicesAsync(listingId, listingDto.Latitude, listingDto.Longitude);
            
            foreach (var service in newServices)
            {
                service.Id = Guid.Empty;
                listing.ListingServices.Add(service);
            }    
            
            listing.ListingTags.Clear();
            var tags = listing.ListingServices.Select(s => s.Type).Distinct().ToList();
            
            await listingRepository.UpdateListingAsync(listing, tags);
        }
        else
            await listingRepository.UpdateListingAsync(listing, null);
        
        return NoContent();
    }

    [HttpPatch("IncrementViews/{listingId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> IncrementListingViews(Guid listingId)
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is not { } listing)
            return NotFound();
        
        if (User.GetRole() != "Client" && !string.IsNullOrEmpty(User.GetRole()))
            return Unauthorized();

        listing.Views++;
        await listingRepository.UpdateListingAsync(listing, null);
        return NoContent();
    }

    [HttpDelete("{listingId:guid}")]
    [Authorize(Policy = "WriteListing")]
    public async Task<IActionResult> DeleteListing(Guid listingId)
    {
        if (await listingRepository.GetListingByIdAsync(listingId) is not { } listing)
            return NotFound();
        
        if (listing.AgentUserId != User.GetUserId())
            return Unauthorized();

        if (!listing.Available)
            return BadRequest("L'immobile è già stato venduto");
        
        await listingRepository.DeleteListingAsync(listing);
        return NoContent();
    }

    [HttpGet("GetReport")]
    [Authorize(Policy = "WriteListing")]
    public async Task<IActionResult> GetReport()
    {
        IList<Listing> listings = (IList<Listing>)await listingRepository.GetDetailedListingsAsync(new ListingFilterDto(){AgentId = User.GetUserId()});
        var report = await excelService.GetReportForAgent(listings);
        
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx"); 
    }
}
