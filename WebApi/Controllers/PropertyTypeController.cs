using AutoMapper;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PropertyTypeController(
    IPropertyTypeRepository propertyTypeRepository,
    IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyTypeResponseDto>>> GetPropertyTypes()
    {
        var propertyTypes = await propertyTypeRepository.GetPropertyTypesAsync();
        return Ok(propertyTypes.Select(mapper.Map<PropertyTypeResponseDto>));
    }
    
    [HttpGet("{propertyTypeId:guid}")]
    public async Task<ActionResult<PropertyTypeResponseDto>> GetPropertyTypeById(Guid propertyTypeId)
    {
        if (await propertyTypeRepository.GetPropertyTypeByIdAsync(propertyTypeId) is { } propertyType)
            return Ok(mapper.Map<PropertyTypeResponseDto>(propertyType));
        
        return NotFound();
    }
    
    [HttpGet("ByCode/{code}")]
    public async Task<ActionResult<PropertyTypeResponseDto>> GetPropertyTypeByCode(string code)
    {
        if (await propertyTypeRepository.GetPropertyTypeByCodeAsync(code) is { } propertyType)
            return Ok(mapper.Map<PropertyTypeResponseDto>(propertyType));
        
        return NotFound();
    }
    
    [HttpPost]
    public async Task<ActionResult<PropertyTypeResponseDto>> CreatePropertyType([FromBody] PropertyTypeRequestDto request)
    {
        var propertyType = mapper.Map<PropertyType>(request);
        await propertyTypeRepository.AddPropertyTypeAsync(propertyType);
        return CreatedAtAction(nameof(GetPropertyTypeById), new { propertyTypeId = propertyType.Id }, mapper.Map<PropertyTypeResponseDto>(propertyType));
    }
}