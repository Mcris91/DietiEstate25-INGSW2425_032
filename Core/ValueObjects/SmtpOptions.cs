using System.Text.Json.Serialization;

namespace DietiEstate.Core.ValueObjects;

public class SmtpOptions
{
    public string Server { get; set; } = string.Empty;
    
    public string Port { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string FromEmail { get; set; } = string.Empty;

    public string FromName { get; set; } = string.Empty;
}