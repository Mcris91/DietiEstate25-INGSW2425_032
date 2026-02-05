using System.Net.Http.Json;
using System.Text.Json;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Core.Entities.ListingModels;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace DietiEstate.Infrastracture.Services;

public class GeoapifyService(
    HttpClient httpClient,
    IConfiguration config)
{
    private readonly string _apiKey = config["GEOAPIFY_KEY"];

    public async Task<List<Service>> GetNearbyServicesAsync(Guid listingId, double listingLat, double listingLon, int radiusMetri = 1000)
    {
        var categories = "commercial.supermarket,education.school,public_transport,healthcare";
        string lonStr = listingLon.ToString(CultureInfo.InvariantCulture);
        string latStr = listingLat.ToString(CultureInfo.InvariantCulture);

        var url = $"https://api.geoapify.com/v2/places?categories={categories}&conditions=named&filter=circle:{lonStr},{latStr},{radiusMetri}&bias=proximity:{lonStr},{latStr}&lang=it&limit=20&apiKey={_apiKey}";

        using JsonDocument doc = await httpClient.GetFromJsonAsync<JsonDocument>(url);
        
        if (!doc.RootElement.TryGetProperty("features", out JsonElement features))
        {
            return [];
        }

        var services = features.EnumerateArray().Select(f =>
        {
            var props = f.GetProperty("properties");
            var coords = f.GetProperty("geometry").GetProperty("coordinates");

            return new Service
            {
                Id = Guid.NewGuid(),
                ListingId = listingId,
                Name = props.TryGetProperty("name", out var n) ? n.GetString() : "Servizio senza nome",
                Address = props.TryGetProperty("street", out var s) ? s.GetString() : "Indirizzo non disponibile",
                Type = props.GetProperty("categories")[0].GetString(),
                Distance = props.GetProperty("distance").GetDouble(),
                Longitude = coords[0].GetDouble(),
                Latitude = coords[1].GetDouble()
            };
        }).ToList();
        
        return services;
    }
}