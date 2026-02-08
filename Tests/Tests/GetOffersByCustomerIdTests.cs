using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using Moq;

namespace DietiEstate.Tests.Tests;

public class GetOffersByCustomerIdTests
{
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IListingRepository> _mockListingRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    
    private readonly OfferController _controller;
    
    public GetOffersByCustomerIdTests()
    {
        _mockOfferRepository = new Mock<IOfferRepository>();
        _mockListingRepository = new Mock<IListingRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        
        _controller = new OfferController(
            _mockOfferRepository.Object,
            _mockListingRepository.Object,
            _mockUserRepository.Object,
            _mockMapper.Object);
    }
    
    [Fact]
    public async Task TC1_BothPageNumberAndPageSizeNull_ReturnsBadRequest()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var filterDto = new OfferFilterDto();
        int? pageNumber = null;
        int? pageSize = null;

        // Act
        var result = await _controller.GetOffersByCustomerId(
            customerId, 
            filterDto, 
            pageNumber, 
            pageSize
        );

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.Value.Should().BeEquivalentTo(new 
        { 
            error = "Both pageNumber and pageSize must be provided for pagination." 
        });
    }
    
}