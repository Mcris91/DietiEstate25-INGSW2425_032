namespace DietiEstate.Shared.Dtos.Responses;

public class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}