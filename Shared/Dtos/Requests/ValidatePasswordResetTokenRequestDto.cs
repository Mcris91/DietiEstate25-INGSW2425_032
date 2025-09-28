using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Dtos.Requests;

public class ValidatePasswordResetTokenRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public int Token { get; set; }
}