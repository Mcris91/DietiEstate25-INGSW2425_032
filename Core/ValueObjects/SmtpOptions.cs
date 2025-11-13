using System.Text.Json.Serialization;

namespace DietiEstate.Core.ValueObjects;

public class SmtpOptions
{
    [JsonPropertyName("SERVER")]
    public string Server { get; init; } = string.Empty;
    
    [JsonPropertyName("PORT")]
    public string Port { get; init; } = string.Empty;
    
    [JsonPropertyName("USERNAME")]
    public string Username { get; init; } = string.Empty;
    
    [JsonPropertyName("PASSWORD")]
    public string Password { get; init; } = string.Empty;
    
    [JsonPropertyName("FROMEMAIL")]
    public string FromEmail { get; init; } = string.Empty;

    [JsonPropertyName("FROMNAME")] 
    public string FromName { get; init; } = string.Empty;
}