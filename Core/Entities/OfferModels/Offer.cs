using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Enums;

namespace DietiEstate.Core.Entities.OfferModels;

public class Offer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public decimal Value { get; set; }
    
    [Required]
    public DateTimeOffset Date { get; set; } =  DateTimeOffset.Now;

    [Required] 
    public OfferStatus Status { get; set; } = OfferStatus.Pending;
    
    [Required]
    [ForeignKey(nameof(Offer))]
    public Guid FirstOfferId { get; set; }
    
    [Required]
    [ForeignKey(nameof(User))]
    public Guid CustomerId { get; set; }
    
    [Required]
    [ForeignKey(nameof(User))]
    public Guid AgentId { get; set; }
    
    [Required]
    public Guid ListingId { get; set; }
    
    [ForeignKey(nameof(ListingId))]
    public virtual Listing Listing { get; set; }
    
    [Required]
    
    public virtual User Customer { get; set; }
    
    public virtual Offer? FirstOffer { get; set; }
}