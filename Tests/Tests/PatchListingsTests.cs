using AutoMapper;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Constants;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastracture.Services;
using DietiEstate.WebApi.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class PatchListingsTests
{
    private readonly Mock<IListingRepository> _mockListingRepository;
    private readonly Mock<IPropertyTypeRepository> _mockPropertyTypeRepository;
    private readonly Mock<IMinioService>  _mockMinioService;
    private readonly Mock<GeoapifyService> _mockGeoapifyService;
    private readonly Mock<IExcelService>  _mockExcelService;
    private readonly Mock<IMapper> _mockMapper;
    
    private readonly ListingController _controller;

    public PatchListingsTests()
    {
        _mockListingRepository = new Mock<IListingRepository>();
        _mockPropertyTypeRepository = new Mock<IPropertyTypeRepository>();
        _mockMinioService = new Mock<IMinioService>();
        _mockGeoapifyService = new Mock<GeoapifyService>();
        _mockExcelService = new Mock<IExcelService>();
        _mockMapper = new Mock<IMapper>();
        
        _controller = new ListingController(
            _mockListingRepository.Object,
            _mockPropertyTypeRepository.Object,
            _mockMinioService.Object,
            _mockGeoapifyService.Object,
            _mockExcelService.Object,
            _mockMapper.Object);
    }
    
    // TC1: GUID valido non esistente + patch valido = NotFound
    [Fact]
    public async Task PatchListing_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var patchDocument = new JsonPatchDocument<ListingRequestDto>();
        patchDocument.Replace(l => l.Name, "Updated Name");

        _mockListingRepository
            .Setup(r => r.GetListingByIdAsync(nonExistentId))
            .ReturnsAsync((Listing?)null);
        
        // Act
        var result = await _controller.PatchListing(nonExistentId, patchDocument);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _mockListingRepository.Verify(r => r.UpdateListingAsync(It.IsAny<Listing>()), Times.Never);
    }

}