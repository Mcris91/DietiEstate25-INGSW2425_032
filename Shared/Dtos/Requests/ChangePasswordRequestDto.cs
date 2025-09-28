using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Dtos.Requests;

public class ChangePasswordRequestDto
{
    [Required]
    public string OldPassword { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string NewPasswordConfirm { get; set; } = string.Empty;
}