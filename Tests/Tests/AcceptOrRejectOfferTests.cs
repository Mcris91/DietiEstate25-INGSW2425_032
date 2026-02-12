using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Constants;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Enums;
using DietiEstate.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class AcceptOrRejectOfferTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly Mock<IListingRepository> _listingRepoMock;
    private readonly IMapper _realMapper;
    private readonly BookingController _controller;

    public AcceptOrRejectOfferTests()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _listingRepoMock = new Mock<IListingRepository>();

        
        var config = new MapperConfiguration(cfg => {
            
            cfg.CreateMap<Booking, BookingFilterDto>(); 
            cfg.CreateMap<Listing, ListingFilterDto>();
        });
        _realMapper = config.CreateMapper();
        
        _controller = new BookingController(
            _bookingRepoMock.Object, 
            _listingRepoMock.Object, 
            _realMapper
        );
    }

    [Theory]
    [InlineData(true, BookingStatus.Accepted)]
    [InlineData(false, BookingStatus.Rejected)]
    public async Task AcceptOrRejectOffer_FullValidFlow_ReturnsOk(bool accept, BookingStatus expectedStatus)
    {
        // ARRANGE
        var bId = Guid.NewGuid();
        var lId = Guid.NewGuid();
        
        var booking = new Booking 
        { 
            Id = bId, 
            Status = BookingStatus.Pending, 
            ListingId = lId 
        };
        
        var listing = new Listing 
        { 
            Id = lId, 
            Available = true 
        };

        // 1. Setup Lettura: restituiamo gli oggetti creati sopra
        _bookingRepoMock.Setup(r => r.GetBookingByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(booking);

        _listingRepoMock.Setup(r => r.GetListingByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(listing);

        // 2. Setup Scrittura: FONDAMENTALE. Ritorna un Task completato.
        // Se manca questo, l'await nel controller riceve null e lancia NullReferenceException.
        _bookingRepoMock.Setup(r => r.UpdateBookingAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _controller.AcceptOrRejectOffer(bId, accept);

        // ASSERT
        Assert.IsType<OkResult>(result);
        Assert.Equal(expectedStatus, booking.Status);
    }
    
    [Fact]
    public async Task AcceptOrRejectOffer_BookingNotFound_ReturnsNotFound()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingRepoMock.Setup(r => r.GetBookingByIdAsync(bookingId)).ReturnsAsync((Booking)null);

        // Act
        var result = await _controller.AcceptOrRejectOffer(bookingId, true);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task AcceptOrRejectOffer_StatusNotPending_ReturnsUnauthorized()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = new Booking { Id = bookingId, Status = BookingStatus.Accepted }; 
        _bookingRepoMock.Setup(r => r.GetBookingByIdAsync(bookingId)).ReturnsAsync(booking);

        // Act
        var result = await _controller.AcceptOrRejectOffer(bookingId, true);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
    
    [Fact]
    public async Task AcceptOrRejectOffer_ListingNotAvailable_ReturnsUnauthorized()
    {
        // Arrange
        Guid bId = Guid.NewGuid(); 
        Guid lId = Guid.NewGuid(); 
    
        // Inizializziamo l'oggetto con lo stato Pending ma Listing NON disponibile
        var booking = new Booking 
        { 
            Id = bId, 
            Status = BookingStatus.Pending, 
            ListingId = lId 
        };
    
        var listing = new Listing 
        { 
            Id = lId, 
            Available = false 
        };

        // Configurazione Mock
        _bookingRepoMock.Setup(r => r.GetBookingByIdAsync(bId))
            .ReturnsAsync(booking);

        _listingRepoMock.Setup(r => r.GetListingByIdAsync(lId))
            .ReturnsAsync(listing);

        // Act
        var result = await _controller.AcceptOrRejectOffer(bId, true);

        // Assert
        // Secondo la logica: if (!listing.Available) return Unauthorized();
        Assert.IsType<UnauthorizedResult>(result);
    }
    
}