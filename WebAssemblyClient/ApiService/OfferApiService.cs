using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class OfferApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<string?> PostOfferAsync(OfferRequestDto offerDto)
    {
        var response = await httpClient.PostAsJsonAsync("", offerDto);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
    
    public async Task<string?> AcceptOrRejectOfferAsync(Guid offerId, bool accept)
    {
        var response = await httpClient.PutAsync($"AcceptOrRejectOffer/{offerId}/{accept}", null);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
    
    public async Task<string?> AcceptOrRejectCounterOfferAsync(Guid offerId, bool accept)
    {
        var response = await httpClient.PutAsync($"AcceptOrRejectCounterOffer/{offerId}/{accept}", null);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
    
    public async Task<string?> DeleteOfferAsync(Guid offerId)
    {
        var response = await httpClient.DeleteAsync($"{offerId}");
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
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
    
    public async Task<PagedResponseDto<OfferResponseDto>> GetOffersByCustomerIdAsync(
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

        var uri = $"GetByCustomerId/{queryString}";
        return await GetAsync<PagedResponseDto<OfferResponseDto>>(uri);
    }
    
    public async Task<PagedResponseDto<OfferResponseDto>> GetOfferHistoryAsync(
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

    public async Task<OfferAgentCountersResponseDto> GetOffersAgentCountersAsync()
    {
        return await GetAsync<OfferAgentCountersResponseDto>("GetTotalOffers/");
    }
}