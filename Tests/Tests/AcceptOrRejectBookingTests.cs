using System.Net;
using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Infrastructure.Data;
using DietiEstate.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class AcceptOrRejectBookingTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AcceptOrRejectBooking_WithValidData_ReturnsOk()
    {
        var bookingId = Guid.NewGuid();
        
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DietiEstateDbContext>();
            dbContext.Booking.Add(new Booking 
            {
                Id = bookingId
            });
            await dbContext.SaveChangesAsync();
        }
        
        var accept = true;

        var url = $"/api/v1/booking/AcceptOrRejectBooking/{bookingId}/{accept}";

        var emptyBody = new StringContent(""); 
    
        var response = await _client.PutAsync(url, emptyBody);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AcceptOrRejectBooking_WithValidData_Returns404NotFound()
    {
        var bookingId = Guid.NewGuid();
        var accept = true;

        var url = $"/api/v1/booking/AcceptOrRejectBooking/{bookingId}/{accept}";

        var emptyBody = new StringContent(""); 
    
        var response = await _client.PutAsync(url, emptyBody);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}