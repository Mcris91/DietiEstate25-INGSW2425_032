using System.Net;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Infrastructure.Data;
using DietiEstate.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class AcceptOrRejectOfferTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AcceptOrRejectOffer_WithValidData_ReturnsOk()
    {
        var offerId = Guid.NewGuid();
        
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DietiEstateDbContext>();
            dbContext.Offer.Add(new Offer 
            {
                Id = offerId
            });
            await dbContext.SaveChangesAsync();
        }
        
        var accept = true;

        var url = $"/api/offers/AcceptOrRejectOffer/{offerId}/{accept}";

        var emptyBody = new StringContent(""); 
    
        var response = await _client.PutAsync(url, emptyBody);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AcceptOrRejectOffer_WithValidData_Returns404NotFound()
    {
        var offerId = Guid.NewGuid();
        var accept = true;

        var url = $"/api/offers/AcceptOrRejectOffer/{offerId}/{accept}";

        var emptyBody = new StringContent(""); 
    
        var response = await _client.PutAsync(url, emptyBody);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}