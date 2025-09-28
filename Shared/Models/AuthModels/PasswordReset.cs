using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.AuthModels;

public class PasswordReset
{
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(6)]
    public int Token { get; set; }
}