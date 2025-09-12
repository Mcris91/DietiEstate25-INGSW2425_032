using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietiEstate.Shared.Models.ListingModels;

public class Image
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Url]
    public string ImageUrl { get; set; } = string.Empty;
}