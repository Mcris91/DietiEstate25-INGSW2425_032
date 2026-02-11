using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetUsersTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    
    
    private readonly UserController _controller;

    public GetUsersTests()
    {
        _mapperMock = new Mock<IMapper>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        
        _controller = new UserController(
            _mapperMock.Object,
            _passwordServiceMock.Object,
            _userRepositoryMock.Object);
    }

    // TEST VALIDI (Weak Equivalence)
    [Theory]
    [InlineData(1, 10)]
    [InlineData(null, null)]
    public async Task GetUsers_WithValidPagination_ReturnsOk(int? pageNumber, int? pageSize)
    {
        
        _userRepositoryMock
            .Setup(x => x.GetUsersAsync(
                It.IsAny<UserFilterDto>(), 
                It.IsAny<int?>(), 
                It.IsAny<int?>()))
            .ReturnsAsync(new List<User>()); 

        
        _mapperMock
            .Setup(m => m.Map<UserResponseDto>(It.IsAny<object>()))
            .Returns(new UserResponseDto());

        
        var result = await _controller.GetUsers(new UserFilterDto(), pageNumber, pageSize);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }
    
    // TEST NON VALIDI (Robustness)

    [Fact] // TC3: XOR Error (uno null, l'altro no)
    public async Task GetUsers_WhenOnlyOnePaginationParamProvided_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetUsers(new UserFilterDto(), 1, null);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.ToString().Should().Contain("Both pageNumber and pageSize must be provided");
    }
    
    [Theory] // TC4 & TC5: Valori minori o uguali a zero
    [InlineData(0, 10)]
    [InlineData(1, -5)]
    [InlineData(-1, -1)]
    public async Task GetUsers_WithInvalidValues_ReturnsBadRequest(int? pageNumber, int? pageSize)
    {
        // Act
        var result = await _controller.GetUsers(new UserFilterDto(), pageNumber, pageSize);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.ToString().Should().Contain("must be greater than zero");
    }
    
}
