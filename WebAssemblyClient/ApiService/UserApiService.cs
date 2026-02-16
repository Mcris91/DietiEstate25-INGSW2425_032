using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class UserApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<PagedResponseDto<UserResponseDto>> GetEmployeesByAgencyId(
        UserFilterDto filterDto,
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
        return await GetAsync<PagedResponseDto<UserResponseDto>>(uri);
    }
    
    public async Task<string?> Creategent(UserRequestDto employee)
    {
        var response = await httpClient.PostAsJsonAsync("create-agent", employee);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
    
    public async Task<string?> CreateSupportAdmin(UserRequestDto employee)
    {
        var response = await httpClient.PostAsJsonAsync("create-support-admin", employee);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }
}