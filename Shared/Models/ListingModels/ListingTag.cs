using System.ComponentModel.DataAnnotations.Schema;

namespace DietiEstate.Shared.Models.ListingModels;

public class ListingTag
{
    public Guid ListingId { get; set; }
    public Guid ServiceId { get; set; }
    
    [ForeignKey(nameof(ListingId))]
    public virtual Listing Listing { get; set; } = null!;
    [ForeignKey(nameof(ServiceId))]
    public virtual Tag Tag { get; set; } = null!;
}
