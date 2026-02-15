using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class BookingApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<string?> PostBookingAsync(BookingRequestDto bookingDto)
    {
        var response = await httpClient.PostAsJsonAsync("", bookingDto);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
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
        var uri = $"GetByAgentId/{queryString}";
        return await GetAsync<PagedResponseDto<BookingResponseDto>>(uri);
    }
    
    public async Task<PagedResponseDto<BookingResponseDto>> GetBookingsByClientIdAsync(
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
        var uri = $"GetByClientId/{queryString}";
        return await GetAsync<PagedResponseDto<BookingResponseDto>>(uri);
    }
    
    public async Task<BookingAgentCountersResponseDto> GetBookingsAgentCountersAsync()
    {
        var uri = "GetTotalBookings";
        return await GetAsync<BookingAgentCountersResponseDto>(uri);
    }
    
    public async Task<string?> AcceptOrRejectBookingAsync(Guid bookingId, bool accept)
    {
        var response = await httpClient.PutAsync($"AcceptOrRejectBooking/{bookingId}/{accept}", null);
        
        if (response.IsSuccessStatusCode)
            return "";
                
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
                
        return null;
    }
    
    public async Task<string?> DeleteBookingAsync(Guid bookingId)
    {
        var response = await httpClient.DeleteAsync($"{bookingId}");
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
}