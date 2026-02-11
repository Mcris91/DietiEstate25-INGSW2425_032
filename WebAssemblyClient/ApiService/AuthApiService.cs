using System.Net;
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

        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<LoginResponseDto>() : null;
    }

    public async Task<string?> Register(UserRequestDto registerRequest)
    {
        var response = await httpClient.PostAsJsonAsync("signup", registerRequest);
        
        if (response.IsSuccessStatusCode)
            return "";
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();
        
        return null;
    }

    public async Task Logout()
    {
        await httpClient.PostAsync("logout", null);
    }

    public async Task<string?> RegisterAgency(RegisterAgencyDto registerAgencyRequest)
    {
        var response = await httpClient.PostAsJsonAsync("register-agency", registerAgencyRequest);

        if (response.IsSuccessStatusCode)
            return "";

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();

        return null;
    }

    public async Task<string> SendForgotPasswordRequest(ForgotPasswordRequestDto forgotPasswordRequest)
    {
        var response = await httpClient.PostAsJsonAsync("forgot-password", forgotPasswordRequest);

        if (response.IsSuccessStatusCode)
            return "";

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();

        return "";
    }

    public async Task<string> SendValidationCodeRequest(ValidatePasswordResetTokenRequestDto requestDto)
    {
        var response = await httpClient.PostAsJsonAsync("validate-reset-token", requestDto);

        if (response.IsSuccessStatusCode)
            return "";

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();

        return "";
    }
    
    public async Task<string> SendPasswordResetRequest(ResetPasswordRequestDto requestDto)
    {
        var response = await httpClient.PostAsJsonAsync("reset-password", requestDto);

        if (response.IsSuccessStatusCode)
            return "";

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return await response.Content.ReadAsStringAsync();

        return "";
    }
}