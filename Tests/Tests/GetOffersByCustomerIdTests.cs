using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.OfferModels;
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
    public async Task TC1_BothPageNumberAndPageSizeNull_ReturnsOkWithoutPagination()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var filterDto = new OfferFilterDto();
        int? pageNumber = null;
        int? pageSize = null;

        var mockOffers = new List<Offer>
        {
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId },
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId },
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId }
        }.AsEnumerable();

        var mockOfferDtos = new List<OfferResponseDto>
        {
            new OfferResponseDto { Id = mockOffers.First().Id },
            new OfferResponseDto { Id = mockOffers.Skip(1).First().Id },
            new OfferResponseDto { Id = mockOffers.Last().Id }
        };

        _mockOfferRepository
            .Setup(r => r.GetOffersByCustomerIdAsync(customerId, filterDto))
            .ReturnsAsync(mockOffers);

        _mockMapper
            .Setup(m => m.Map<OfferResponseDto>(It.IsAny<Offer>()))
            .Returns((Offer offer) => mockOfferDtos.First(dto => dto.Id == offer.Id));

        // Act
        var result = await _controller.GetOffersByCustomerId(
            customerId, 
            filterDto, 
            pageNumber, 
            pageSize
        );

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
    
        var pagedResponse = okResult.Value as PagedResponseDto<OfferResponseDto>;
        pagedResponse.Should().NotBeNull();
        pagedResponse.Items.Should().HaveCount(3);
    
        // Quando non c'è paginazione, potrebbe restituire tutti gli elementi
        // Verifica che il risultato contenga gli elementi attesi
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    [InlineData(-5, -5)]
    public async Task TC2_PageNumberOrPageSizeNotGreaterThanZero_ReturnsBadRequest(
        int pageNumber, 
        int pageSize)
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var filterDto = new OfferFilterDto();

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
            error = "Both pageNumber and pageSize must be greater than zero." 
        });
    }
    
    [Fact]
    public async Task TC3_ValidParametersWithFilter_ReturnsOkWithPagedData()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var filterDto = new OfferFilterDto 
        { 
            // Popola con proprietà appropriate
        };
        int pageNumber = 1;
        int pageSize = 10;

        var mockOffers = new List<Offer>
        {
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId },
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId },
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId }
        }.AsEnumerable();

        _mockOfferRepository
            .Setup(r => r.GetOffersByCustomerIdAsync(customerId, filterDto))
            .ReturnsAsync(mockOffers);

        // Act
        var result = await _controller.GetOffersByCustomerId(
            customerId, 
            filterDto, 
            pageNumber, 
            pageSize
        );

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var pagedResponse = okResult.Value as PagedResponseDto<OfferResponseDto>;
        pagedResponse.Should().NotBeNull();
        pagedResponse.PageSize.Should().Be(pageSize);
        pagedResponse.PageNumber.Should().Be(pageNumber);
        pagedResponse.Items.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TC4_ValidParametersWithNullFilter_ReturnsOkWithPagedData()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        OfferFilterDto filterDto = null;
        int pageNumber = 1;
        int pageSize = 20;

        var mockOffers = new List<Offer>
        {
            new Offer { Id = Guid.NewGuid(), CustomerId = customerId }
        }.AsEnumerable();

        _mockOfferRepository
            .Setup(r => r.GetOffersByCustomerIdAsync(customerId, null))
            .ReturnsAsync(mockOffers);

        // Act
        var result = await _controller.GetOffersByCustomerId(
            customerId, 
            filterDto, 
            pageNumber, 
            pageSize
        );

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var pagedResponse = okResult.Value as PagedResponseDto<OfferResponseDto>;
        pagedResponse.Should().NotBeNull();
        pagedResponse.PageSize.Should().Be(pageSize);
        pagedResponse.PageNumber.Should().Be(pageNumber);
        pagedResponse.Items.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task TC5_EmptyResultSet_ReturnsOkWithEmptyPagedData()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var filterDto = new OfferFilterDto();
        int pageNumber = 1;
        int pageSize = 10;

        _mockOfferRepository
            .Setup(r => r.GetOffersByCustomerIdAsync(customerId, filterDto))
            .ReturnsAsync(Enumerable.Empty<Offer>());

        // Act
        var result = await _controller.GetOffersByCustomerId(
            customerId, 
            filterDto, 
            pageNumber, 
            pageSize
        );

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var pagedResponse = okResult.Value as PagedResponseDto<OfferResponseDto>;
        pagedResponse.Should().NotBeNull();
        pagedResponse.Items.Should().BeEmpty();
    }
}