using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

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