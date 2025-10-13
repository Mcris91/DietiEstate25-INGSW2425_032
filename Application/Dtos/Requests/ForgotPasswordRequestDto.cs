using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Application.Dtos.Requests;

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}