using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Core.Entities.ListingModels;

public class Service
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<Listing> Listings { get; set; } = [];
}