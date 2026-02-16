using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class ListingApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{

    public async Task<string?> CreateListingAsync(ListingRequestDto listingDto)
    {
        var response = await httpClient.PostAsJsonAsync("", listingDto);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
    public async Task<ListingResponseDto> GetListingByIdAsync(Guid listingId)
    {
        var uri = $"{listingId}";
        return await GetAsync<ListingResponseDto>(uri);
    }

    public async Task IncrementListingViews(Guid listingId)
    {
        await httpClient.PatchAsync($"IncrementViews/{listingId}", null);
    }
    
    public async Task<PagedResponseDto<ListingResponseDto>> GetListingsAsync(
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
        return await GetAsync<PagedResponseDto<ListingResponseDto>>(uri);
    }
    
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
        
        var uri = $"Dashboard/{queryString}";
        return await GetAsync<PagedResponseDto<ListingResponseDto>>(uri);
    }
    
    
    public async Task<PagedResponseDto<ListingResponseDto>> GetGetRecentListingsAsync(IList<Guid> listingIds,
        int? pageNumber,
        int? pageSize)
    {
        var queryString = string.Join("&", listingIds.Select(id => $"listingIdsList={id}"));
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
        
        var uri = $"RecentListings?{queryString}";
        return await GetAsync<PagedResponseDto<ListingResponseDto>>(uri);
    }
    
    public async Task<ListingAgentCountersResponseDto> GetListingAgentCountersAsync()
    {

        var uri = "GetAgentCounters";
        return await GetAsync<ListingAgentCountersResponseDto>(uri);
    }

    public async Task<byte[]> GetReportAsync()
    {
        var uri = "GetReport";
        var response = await httpClient.GetAsync(uri);
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<string?> UpdateListingAsync(Guid listingId, ListingRequestDto listing)
    {
        var response = await httpClient.PatchAsJsonAsync($"{listingId}", listing);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }

    public async Task<string?> DeleteListingAsync(Guid listingId)
    {
        var response = await httpClient.DeleteAsync($"{listingId}");
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
}