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
    
    public async Task AddEmployee(UserRequestDto employee)
    {
        await httpClient.PostAsJsonAsync("create-agent", employee);
    }
}