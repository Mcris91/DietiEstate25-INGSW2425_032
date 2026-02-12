using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastructure.Services;
using DietiEstate.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetListingsTests
{
    private readonly Mock<IListingRepository> _listingRepositoryMock = new();
    private readonly Mock<IPropertyTypeRepository> _propertyTypeRepositoryMock = new();
    private readonly Mock<IMinioService> _minioMock = new();
    private readonly Mock<IExcelService> _excelMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    
    
    private readonly ListingController _controller;

    public GetListingsTests()
    {
        _controller = new ListingController(
            _listingRepositoryMock.Object,
            _propertyTypeRepositoryMock.Object,
            _minioMock.Object,
            null!,
            _excelMock.Object,
            _mapperMock.Object);
    }
    
    [Fact] // TC1: Normal - Valid (Entrambi Null)
    public async Task GetListings_BothNull_ReturnsOk()
    {
        _listingRepositoryMock.Setup(r => r.GetListingsAsync(It.IsAny<ListingFilterDto>(), null, null))
            .ReturnsAsync(new List<Listing>());

        var result = await _controller.GetListings(new ListingFilterDto(), null, null);

        Assert.IsType<OkObjectResult>(result.Result);
    }
    
    [Theory] // TC2: Weak Equivalence - Invalid (XOR Logic)
    [InlineData(1, null)]
    [InlineData(null, 10)]
    public async Task GetListings_PartialPagination_ReturnsBadRequest(int? pageNum, int? size)
    {
        var result = await _controller.GetListings(new ListingFilterDto(), pageNum, size);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Both pageNumber and pageSize must be provided", badRequest.Value.ToString());
    }
    
    [Fact] // TC4: Normal - Valid (Paginazione corretta)
    public async Task GetListings_ValidPagination_ReturnsOk()
    {
        var listings = new List<Listing> { new Listing { FeaturedImage = "test.png" } };
        _listingRepositoryMock.Setup(r => r.GetListingsAsync(It.IsAny<ListingFilterDto>(), 1, 10))
            .ReturnsAsync(listings);
        
        _mapperMock.Setup(m => m.Map<ListingResponseDto>(It.IsAny<Listing>()))
            .Returns(GetValidDto());

        var result = await _controller.GetListings(new ListingFilterDto(), 1, 10);

        Assert.IsType<OkObjectResult>(result.Result);
    }
    
    private ListingResponseDto GetValidDto()
    {
        return new ListingResponseDto 
        { 
            Name = "Villa Borghese",
            Description = "Villa Borghese una dimora a pochi passi dal mare, ampio giordino e grande terrazza",
            FeaturedImage = "VillaBorghese.jpg",
            Address = "Via Roma 1",
            Latitude = 40.8,
            Longitude = 14.2,
            City = "Napoli",
            EnergyClass = "A",
            Type = "Vendita"
        };
    }
}