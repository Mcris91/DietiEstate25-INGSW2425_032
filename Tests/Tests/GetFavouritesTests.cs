using System.Net;
using System.Net.Http.Headers;
using DietiEstate.Tests;
using DietiEstate.WebApi;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetFavouritesTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    // Test1 - CE1, CE4
    [Fact]
    public async Task Test1_CE1_CE4()
    {
        // Arrange
        int? pageNumber = 1;
        int? pageSize = 10;

        // Act
        var response = await _client.GetAsync($"/api/favourites?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Test2 - CE1, CE5
    [Fact]
    public async Task Test2_CE1_CE5()
    {
        // Arrange
        int? pageNumber = 1;
        int? pageSize = 0;

        // Act
        var response = await _client.GetAsync($"/api/favourites?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Test3 - CE2, CE4
    [Fact]
    public async Task Test3_CE2_CE4()
    {
        // Arrange
        int? pageNumber = 0;
        int? pageSize = 10;

        // Act
        var response = await _client.GetAsync($"/api/favourites?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}