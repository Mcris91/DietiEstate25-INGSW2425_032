using System.Text.Json;
using DietiEstate.WebClient.Data.Requests;
using DietiEstate.WebClient.Data.Responses;

namespace DietiEstate.WebClient.ApiService;

public class ListingApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<PagedResponseDto<ListingResponseDto>> GetListingsByAgentIdAsync(
        ListingFilterDto filterDto,
        int? pageNumber,
        int? pageSize)
    {
        var queryString = filterDto.ToQueryString();
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
        Console.WriteLine(uri); 
        return await GetAsync<PagedResponseDto<ListingResponseDto>>(uri);
    }
}