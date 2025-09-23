using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.UserModels;

public class UserSession
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiry { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastAccessed { get; set; }
}