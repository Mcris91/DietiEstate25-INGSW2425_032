using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietiEstate.Shared.Models.ListingModels;

public class ListingImage
{
    [Required]
    public Guid ListingId { get; set; }

    [Required]
    public Guid ImageId { get; set; }
    
    [ForeignKey(nameof(ListingId))]
    public virtual Listing Listing { get; set; } = null!;
    [ForeignKey(nameof(ImageId))]
    public virtual Image Image { get; set; } = null!;
}