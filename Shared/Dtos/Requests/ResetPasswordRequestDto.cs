using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Dtos.Requests;

public class ResetPasswordRequestDto
{
    [Required]
    public Guid Token { get; set; } = Guid.NewGuid();
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string PasswordConfirm { get; set; } = string.Empty;
}