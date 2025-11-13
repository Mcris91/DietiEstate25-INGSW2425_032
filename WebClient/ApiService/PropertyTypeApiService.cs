using System.Text.Json;
using DietiEstate.WebClient.Data.Responses;

namespace DietiEstate.WebClient.ApiService;

public class PropertyTypeApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<PropertyTypeResponseDto> GetPropertyTypeByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));
        
        var uri = $"ByCode/{code}";
        return await GetAsync<PropertyTypeResponseDto>(uri);
    }
}