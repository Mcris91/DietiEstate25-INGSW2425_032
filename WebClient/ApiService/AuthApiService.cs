using System.Text.Json;
using DietiEstate.WebClient.Data.Requests;
using DietiEstate.WebClient.Data.Responses;

namespace DietiEstate.WebClient.ApiService;

public class AuthApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task<LoginResponseDto?> GoogleLogin(GoogleLoginRequestDto googleLoginRequest)
    {
        return await httpClient.PostAsJsonAsync("google-callback", googleLoginRequest).Result.Content.ReadFromJsonAsync<LoginResponseDto>();
    }
    
    public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequest)
    {
        return await httpClient.PostAsJsonAsync("login", loginRequest).Result.Content.ReadFromJsonAsync<LoginResponseDto>();
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