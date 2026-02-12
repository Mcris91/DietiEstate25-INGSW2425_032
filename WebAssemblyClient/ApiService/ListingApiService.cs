using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class ListingApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{

    public async Task CreateListingAsync(ListingRequestDto listingDto)
    {
        await httpClient.PostAsJsonAsync("", listingDto);
    }
    public async Task<ListingResponseDto> GetListingByIdAsync(Guid listingId)
    {
        var uri = $"{listingId}";
        return await GetAsync<ListingResponseDto>(uri);
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
        
        var uri = queryString;
        return await GetAsync<PagedResponseDto<ListingResponseDto>>(uri);
    }

    public async Task<ListingAgentCountersResponseDto> GetListingAgentCountersAsync(Guid? agentId)
    {
        if (agentId == null)
            throw new ArgumentException("AgentId cannot be null.", nameof(agentId));
        
        var uri = $"GetAgentCounters/{agentId}";
        return await GetAsync<ListingAgentCountersResponseDto>(uri);
    }

    public async Task<byte[]> GetReportAsync(Guid? agentId)
    {
        var uri = $"GetReport/{agentId}";
        var response = await httpClient.GetAsync(uri);
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task UpdateListingAsync(Guid listingId, ListingRequestDto listing)
    {
        await httpClient.PatchAsJsonAsync($"{listingId}", listing);
    }
}