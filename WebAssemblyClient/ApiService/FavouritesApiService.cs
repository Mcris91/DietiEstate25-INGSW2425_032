using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class FavouritesApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task RemoveFavouriteAsync(Guid listingId)
    {
        await httpClient.DeleteAsync($"{listingId}");
    }

    public async Task CreateFavouriteAsync(Guid listingId)
    {
        await httpClient.PostAsync($"{listingId}", null);
    }

    public async Task<PagedResponseDto<UserFavouritesResponseDto>> GetFavouritesAsync(int? pageNumber,
        int? pageSize)
    {
        var queryString = string.Empty;
        if (pageNumber.HasValue)
        {
            queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
            queryString += $"pageNumber={pageNumber.Value}";
        }

        if (pageSize.HasValue)
        {
            queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
            queryString += $"pageSize={pageSize.Value}";
        }
        
        var uri = queryString;
        
        return await GetAsync<PagedResponseDto<UserFavouritesResponseDto>>(uri);
    }

    public async Task<List<Guid>> GetFavouritesIdList()
    {
        var response = await httpClient.GetAsync("favourites-list");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Guid>>() ?? [];
        }
        return [];
    }
}