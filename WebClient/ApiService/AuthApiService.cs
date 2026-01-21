using System.Text.Json;
using DietiEstate.WebClient.Data.Requests;

namespace DietiEstate.WebClient.ApiService;

public class AuthApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : BaseApiService(httpClient, jsonSerializerOptions)
{
    public async Task Login(LoginRequestDto loginRequest)
    {
        await httpClient.PostAsJsonAsync("login", loginRequest);
    }

    public async Task Register(UserRequestDto registerRequest)
    {
        await httpClient.PostAsJsonAsync("signup", registerRequest);
    }

    public async Task Logout()
    {
        await httpClient.PostAsync("logout", null);
    }
}