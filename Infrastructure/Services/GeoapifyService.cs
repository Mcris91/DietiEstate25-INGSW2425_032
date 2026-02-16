using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using DietiEstate.Core.Entities.ListingModels;
using Microsoft.Extensions.Configuration;

namespace DietiEstate.Infrastructure.Services;

public class GeoapifyService(
    HttpClient httpClient,
    IConfiguration config)
{
    private readonly string _apiKey = config["GEOAPIFY_KEY"];

    public async Task<List<Service>> GetNearbyServicesAsync(Guid listingId, double listingLat, double listingLon, int radiusMetri = 1000)
    {
        var categories = "leisure.park,education.school,public_transport";
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
            var allCategories = props.GetProperty("categories").EnumerateArray()
                .Select(c => c.GetString())
                .ToList();

            var serviceType = "";
            if (allCategories.Any(c => c.Contains("park"))) serviceType = "Parchi";
            else if (allCategories.Any(c => c.Contains("school"))) serviceType = "Scuole";
            else if (allCategories.Any(c => c.Contains("public_transport") || c.Contains("bus") || c.Contains("subway"))) serviceType = "Trasporti pubblici";

            return new Service
            {
                Id = Guid.NewGuid(),
                ListingId = listingId,
                Name = props.TryGetProperty("name", out var n) ? n.GetString() : "Servizio senza nome",
                Address = props.TryGetProperty("street", out var s) ? s.GetString() : "Indirizzo non disponibile",
                Type = serviceType,
                Distance = props.GetProperty("distance").GetDouble(),
                Longitude = coords[0].GetDouble(),
                Latitude = coords[1].GetDouble()
            };
        })
        .Where(s => s.Name.Length <= 50)
        .ToList();
        
        return services;
    }
}