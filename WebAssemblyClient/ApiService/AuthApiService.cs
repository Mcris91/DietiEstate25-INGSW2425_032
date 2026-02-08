using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.ApiService;

public class AuthApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<LoginResponseDto?> GoogleLogin(GoogleLoginRequestDto googleLoginRequest)
    {
        var response = await httpClient.PostAsJsonAsync("google-callback", googleLoginRequest);
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
    }
    
    public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequest)
    {
        var response = await httpClient.PostAsJsonAsync("login", loginRequest);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
    }

    public async Task Register(UserRequestDto registerRequest)
    {
        await httpClient.PostAsJsonAsync("signup", registerRequest);
    }

    public async Task Logout()
    {
        await httpClient.PostAsync("logout", null);
    }

    public async Task RegisterAgency(RegisterAgencyDto registerAgencyRequest)
    {
        await httpClient.PostAsJsonAsync("register-agency", registerAgencyRequest);

    }
}