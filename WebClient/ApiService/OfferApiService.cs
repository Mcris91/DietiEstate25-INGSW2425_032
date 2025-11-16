using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.WebClient.Data.Responses;
using DietiEstate.Application.Dtos.Requests;

using OfferResponseDto = DietiEstate.Application.Dtos.Responses.OfferResponseDto;

namespace DietiEstate.WebClient.ApiService;

public class OfferApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task PostOfferAsync(OfferRequestDto offerDto)
    {
        await httpClient.PostAsJsonAsync("api/v1/Offer", offerDto);
    }
    
    public async Task AcceptOrRejectOfferAsync(Guid offerId, bool accept)
    {
        await httpClient.PutAsync($"api/v1/Offer/AcceptOrRejectOffer/{offerId}/{accept}", null);
    }
    
    public async Task DeleteOfferAsync(Guid offerId, Guid customerId)
    {
        await httpClient.DeleteAsync($"api/v1/Offer/{offerId}/{customerId}");
    }
    
    public async Task<ActionResult<PagedResponseDto<OfferResponseDto>>> GetOffersByAgentIdAsync(
        Guid agentId,
        OfferFilterDto filterDto,
        int? pageNumber,
        int? pageSize)
    {
        var query = new Dictionary<string, string?>
        {
            ["CustomerFirstName"] = filterDto.CustomerFirstName,
            ["CustomerLastName"] = filterDto.CustomerLastName,
            ["CustomerEmail"] = filterDto.CustomerEmail,
            ["ListingName"] = filterDto.ListingName,
            ["Value"] = filterDto.Value.ToString(),
            ["Date"] = filterDto.Date.ToString(),
            ["Status"] = filterDto.Status.ToString(),
            ["SortBy"] = filterDto.SortBy,
            ["SortOrder"] = filterDto.SortOrder,
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };
        var uri = QueryHelpers.AddQueryString($"api/v1/Offer/GetByAgentId/{agentId}", query);
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

    public async Task<ActionResult<(int Total, int Pending)>> GetTotalOffersAsync(Guid agentId)
    {
        return await GetAsync<(int Total, int Pending)>($"api/v1/Offer/GetTotalOffers/{agentId}");
    }
}