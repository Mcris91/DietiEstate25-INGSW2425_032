using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.Common;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastracture.Services;
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
    [Authorize(Policy = "ReadListing")]
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

    [HttpGet("GetAgentCounters/{agentId:guid}")]
    [Authorize(Policy = "ReadListing")]
    public async Task<IActionResult> GetAgentCounters(Guid agentId)
    {
        var listings = await listingRepository.GetListingsAsync(
            new ListingFilterDto() { AgentId = agentId }, null, null);

        var forRentType = await propertyTypeRepository.GetPropertyTypeByCodeAsync("RENT");
        var forSaleType = await propertyTypeRepository.GetPropertyTypeByCodeAsync("SALE");
        
        if (forRentType == null || forSaleType == null)
            return StatusCode(500, new { error = "Property types 'RENT' and 'SALE' must exist in the database." });

        var listingsEnumerable = listings.ToList();
        
        return Ok(new ListingAgentCountersResponseDto()
        {
            ForRentCount = listingsEnumerable.Count(l => l.TypeId == forRentType.Id),
            ForSaleCount = listingsEnumerable.Count(l => l.TypeId == forSaleType.Id),
        });
    }

    [HttpPost]
    [Authorize(Policy = "WriteListing")]
    public async Task<IActionResult> PostListing([FromBody] ListingRequestDto request)
    {
        var listing = mapper.Map<Listing>(request);

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

        listing.ListingServices = await geoapifyService.GetNearbyServicesAsync(listing.Id, listing.Latitude, listing.Longitude);
        var tags = listing.ListingServices.Select(s => s.Type).Distinct().ToList();
        var type = await propertyTypeRepository.GetPropertyTypeByCodeAsync(request.TypeCode);
        listing.TypeId = type.Id;
        await listingRepository.AddListingAsync(listing, tags);
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

    [HttpGet("GetReport/{agentId:guid}")]
    public async Task<IActionResult> GetReport(Guid agentId)
    {
        IList<Listing> listings = (IList<Listing>)await listingRepository.GetListingsAsync(new ListingFilterDto(){AgentId = agentId}, 0, 1000);
        var report = await excelService.GetReportForAgent(listings);
        
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx"); 
    }
}
