using System.Collections;
using System.Globalization;
using System.Net;
using System.Reflection;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.WebApi;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetOffersByAgentId(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    // Test1 - CE1, CE3, CE6
    // filterDto: not null, pageNumber: > 0, pageSize: > 0
    [Fact]
    public async Task Test1_CE1_CE3_CE6()
    {
        var filterDto = new OfferFilterDto(); // CE1: not null
        var pageNumber = 1; // CE3: > 0
        var pageSize = 10; // CE6: > 0

        var queryString = ObjectToQueryString(filterDto);
        queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
        queryString += $"pageNumber={pageNumber}";
        queryString += $"&pageSize={pageSize}";

        var url = $"/api/v1/offer/GetByAgentId/{queryString}";
    
        var response = await _client.GetAsync(url);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Test2 - CE1, CE3, CE7
    // filterDto: not null, pageNumber: > 0, pageSize: <= 0
    [Fact]
    public async Task Test2_CE1_CE3_CE7()
    {
        var filterDto = new OfferFilterDto();
        var pageNumber = 1;
        var pageSize = 0;

        var queryString = ObjectToQueryString(filterDto);
        queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
        queryString += $"pageNumber={pageNumber}";
        queryString += $"&pageSize={pageSize}";

        var url = $"/api/v1/offer/GetByAgentId/{queryString}";
        
        var response = await _client.GetAsync(url);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Test3 - CE1, CE4, CE6
    // filterDto: not null, pageNumber: <= 0, pageSize: > 0
    [Fact]
    public async Task Test3_CE1_CE4_CE6()
    {
        var filterDto = new OfferFilterDto();
        var pageNumber = 0;
        var pageSize = 10;

        var queryString = ObjectToQueryString(filterDto);
        queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
        queryString += $"pageNumber={pageNumber}";
        queryString += $"&pageSize={pageSize}";

        var url = $"/api/v1/offer/GetByAgentId/{queryString}";
        
        var response = await _client.GetAsync(url);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Test4 - CE2, CE3, CE6
    // filterDto: null, pageNumber: > 0, pageSize: > 0
    [Fact]
    public async Task Test4_CE2_CE3_CE6()
    {
        OfferFilterDto filterDto = null;
        var pageNumber = 1;
        var pageSize = 10;

        var queryString = "";
        queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
        queryString += $"pageNumber={pageNumber}";
        queryString += $"&pageSize={pageSize}";

        var url = $"/api/v1/offer/GetByAgentId/{queryString}";
        
        var response = await _client.GetAsync(url);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Metodo sotto test per gestire le casistiche
    private object Execute(OfferFilterDto filterDto, int pageNumber, int pageSize)
    {
        if (filterDto == null)
            throw new ArgumentNullException(nameof(filterDto));
        
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        
        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

        // Logica di business
        return new object();
    }
    
    private string ObjectToQueryString(object objItem)
    {
        var props = objItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var parts = new List<string>();

        foreach (var prop in props)
        {
            var value = prop.GetValue(objItem);
            if (value == null)
                continue;

            var name = Uri.EscapeDataString(prop.Name);

            if (value is string str)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                parts.Add($"{name}={Uri.EscapeDataString(str)}");
            }
            else if (value is IEnumerable enumerable and not IDictionary)
            {
                parts.AddRange(enumerable.OfType<object>().Select(item => $"{name}={Uri.EscapeDataString(item.ToString()!)}"));
            }
            else if (value is IFormattable formattable)
            {
                parts.Add($"{name}={Uri.EscapeDataString(formattable.ToString(null, CultureInfo.InvariantCulture))}");
            }
            else
            {
                parts.Add($"{name}={Uri.EscapeDataString(value.ToString()!)}");
            }
        }

        if (parts.Count == 0)
            return string.Empty;

        return "?" + string.Join("&", parts);
    }
}