namespace DietiEstate.WebClient.Data.Responses;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
}