using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using DietiEstate.WebClient.Data.Responses;
using DietiEstate.WebClient.Data.Requests;

namespace DietiEstate.WebClient.ApiService;

public class OfferApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task PostOfferAsync(OfferRequestDto offerDto)
    {
        await httpClient.PostAsJsonAsync("", offerDto);
    }
    
    public async Task AcceptOrRejectOfferAsync(Guid offerId, bool accept)
    {
        await httpClient.PutAsync($"AcceptOrRejectOffer/{offerId}/{accept}", null);
    }
    
    public async Task DeleteOfferAsync(Guid offerId, Guid customerId)
    {
        await httpClient.DeleteAsync($"api/v1/Offer/{offerId}/{customerId}");
    }
    
    public async Task<PagedResponseDto<OfferResponseDto>> GetOffersByAgentIdAsync(
        OfferFilterDto filterDto,
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

        var uri = $"GetByAgentId/{queryString}";
        return await GetAsync<PagedResponseDto<OfferResponseDto>>(uri);
    }
    
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOffersByCustomerIdAsync(
        Guid customerId,
        OfferFilterDto filterDto,
        int? pageNumber,
        int? pageSize)
    {
        var query = new Dictionary<string, string?>
        {
            ["ListingName"] = filterDto.ListingName,
            ["Value"] = filterDto.Value.ToString(),
            ["Date"] = filterDto.Date.ToString(),
            ["Status"] = filterDto.Status.ToString(),
            ["SortBy"] = filterDto.SortBy,
            ["SortOrder"] = filterDto.SortOrder,
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };
        var uri = QueryHelpers.AddQueryString($"api/v1/Offer/GetByCustomerId/{customerId}", query);
        return await GetAsync<PagedResponseDto<OfferResponseDto>>(uri);
    }
    
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOfferHistoryAsync(
        Guid offerId,
        int? pageNumber,
        int? pageSize)
    {
        var query = new Dictionary<string, string?>
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };
        var uri = QueryHelpers.AddQueryString($"api/v1/Offer/GetOfferHistory/{offerId}", query);
        return await GetAsync<PagedResponseDto<OfferResponseDto>>(uri);
    }

    public async Task<OfferAgentCountersResponseDto> GetTotalOffersAsync(Guid? agentId)
    {
        if (agentId == null)
            throw new ArgumentException("AgentId cannot be null.", nameof(agentId));
        
        return await GetAsync<OfferAgentCountersResponseDto>($"GetTotalOffers/{agentId}");
    }
}