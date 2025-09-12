using System.ComponentModel.DataAnnotations.Schema;

namespace DietiEstate.Shared.Models.ListingModels;

public class ListingService
{
    public Guid ListingId { get; set; }
    public Guid ServiceId { get; set; }
    
    [ForeignKey(nameof(ListingId))]
    public virtual Listing Listing { get; set; } = null!;
    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}