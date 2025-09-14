namespace DietiEstate.Shared.Dtos.Requests;

public class LoginRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}