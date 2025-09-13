using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.Shared;

public class Image
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;
}