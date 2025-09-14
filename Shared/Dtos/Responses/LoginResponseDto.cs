namespace DietiEstate.Shared.Dtos.Responses;

public class LoginResponseDto
{
    public string Access { get; init; } = string.Empty;
    public string Refresh { get; init; } = string.Empty;
}