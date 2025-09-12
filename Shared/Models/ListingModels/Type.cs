using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.Listing;

public class Type
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string Icon { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}