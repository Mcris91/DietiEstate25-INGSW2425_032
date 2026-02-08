using System.ComponentModel.DataAnnotations;

namespace WebAssemblyClient.Data.Requests;

public class PropertyTypeRequestDto
{
    [Required]
    [MaxLength(50)]
    public string Icon { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Code { get; set; } = string.Empty;
}