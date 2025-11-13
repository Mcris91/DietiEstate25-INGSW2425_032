using System.Text.Json.Serialization;

namespace DietiEstate.Core.ValueObjects;

public class AdminUserTemplate
{
    [JsonPropertyName("FIRSTNAME")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("LASTNAME")]
    public string LastName { get; set; } = string.Empty;
    
    [JsonPropertyName("EMAIL")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("PASSWORD")]
    public string Password { get; set; } = string.Empty;
}