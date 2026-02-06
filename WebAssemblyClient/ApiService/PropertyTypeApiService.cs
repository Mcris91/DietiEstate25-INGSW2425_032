using System.Text.Json;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class PropertyTypeApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<IEnumerable<PropertyTypeResponseDto>> GetPropertyTypesAsync()
    {
        return await GetAsync<IEnumerable<PropertyTypeResponseDto>>("");
    }
    
    public async Task<PropertyTypeResponseDto> GetPropertyTypeByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));
        
        var uri = $"ByCode/{code}";
        return await GetAsync<PropertyTypeResponseDto>(uri);
    }
}