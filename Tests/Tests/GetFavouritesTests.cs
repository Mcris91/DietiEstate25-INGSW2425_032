using System.Net.Http.Headers;
using DietiEstate.Tests;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetFavouritesTests
{
    private readonly CustomWebApplicationFactory _factory;

    public GetFavouritesTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // Test1 - CE1, CE4
    [Fact]
    public async Task Test1_CE1_CE4()
    {
        // Arrange
        int? pageNumber = 1;
        int? pageSize = 10;
        var client = _factory.CreateClient();
        
        // Aggiungi token di autenticazione
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", "your-test-token-here");

        // Act
        var response = await client.GetAsync($"/api/favourites?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    // Test2 - CE1, CE5
    [Fact]
    public async Task Test2_CE1_CE5()
    {
        // Arrange
        int? pageNumber = 1;
        int? pageSize = 0;
        var client = _factory.CreateClient();
        
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", "your-test-token-here");

        // Act
        var response = await client.GetAsync($"/api/favourites?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Test3 - CE2, CE4
    [Fact]
    public async Task Test3_CE2_CE4()
    {
        // Arrange
        int? pageNumber = 0;
        int? pageSize = 10;
        var client = _factory.CreateClient();
        
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", "your-test-token-here");

        // Act
        var response = await client.GetAsync($"/api/favourites?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}