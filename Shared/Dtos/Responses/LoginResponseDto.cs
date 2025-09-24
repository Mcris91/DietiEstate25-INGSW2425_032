namespace DietiEstate.Shared.Dtos.Responses;

/// <summary>
/// Represents the response data structure returned upon successful user login.
/// </summary>
public class LoginResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}