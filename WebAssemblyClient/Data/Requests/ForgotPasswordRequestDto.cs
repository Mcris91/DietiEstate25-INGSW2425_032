using System.ComponentModel.DataAnnotations;

namespace WebAssemblyClient.Data.Requests;

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}