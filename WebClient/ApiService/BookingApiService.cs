using System.Text.Json;
using DietiEstate.WebClient.Data.Requests;
using DietiEstate.WebClient.Data.Responses;

namespace DietiEstate.WebClient.ApiService;

public class BookingApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task PostBookingAsync(BookingRequestDto bookingDto)
    {
        await httpClient.PostAsJsonAsync("", bookingDto);
    }
    
    public async Task<PagedResponseDto<BookingResponseDto>> GetBookingsByAgentIdAsync(
        BookingFilterDto filterDto,
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
        return await GetAsync<PagedResponseDto<BookingResponseDto>>(uri);
    }
    
    public async Task<BookingAgentCountersResponseDto> GetBookingsAgentCountersAsync(Guid? agentId)
    {
        if (agentId == null)
            throw new ArgumentException("AgentId cannot be null.", nameof(agentId));
        
        var uri = $"GetTotalBookings/{agentId}";
        return await GetAsync<BookingAgentCountersResponseDto>(uri);
    }
    
    public async Task AcceptOrRejectBookingAsync(Guid bookingId, bool accept)
    {
        await httpClient.PutAsync($"AcceptOrRejectBooking/{bookingId}/{accept}", null);
    }
}