using System.Text.Json;
using DietiEstate.WebClient.Data.Requests;
using DietiEstate.WebClient.Data.Responses;

namespace DietiEstate.WebClient.ApiService;

public class ListingApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
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
}