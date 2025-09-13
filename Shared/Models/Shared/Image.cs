using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.Shared;

public class Image
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Url]
    public string ImageUrl { get; set; } = string.Empty;
}