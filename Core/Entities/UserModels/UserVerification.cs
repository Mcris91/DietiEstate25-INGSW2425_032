using System.ComponentModel.DataAnnotations;
using DietiEstate.Core.Enums;

namespace DietiEstate.Core.Entities.UserModels;

public class UserVerification
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string Token { get; set; } = Guid.NewGuid().ToString("N");
    
    [Required]
    public VerificationStatus Status { get; set; } = VerificationStatus.Pending;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(60);
    
    [Required]
    public DateTime? VerifiedAt { get; set; }
}