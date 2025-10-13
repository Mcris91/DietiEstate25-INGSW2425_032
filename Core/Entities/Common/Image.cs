using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Core.Entities.Common;

public class Image
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    public virtual ICollection<ListingModels.Listing> Listings { get; set; } = [];
}