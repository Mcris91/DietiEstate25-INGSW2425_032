using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietiEstate.Core.Entities.ListingModels;

public class Service
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;
    
    
    public string Address { get; set; } = string.Empty;
    
    public double Distance { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    [ForeignKey(nameof(Listing))]
    public Guid ListingId { get; set; }
}