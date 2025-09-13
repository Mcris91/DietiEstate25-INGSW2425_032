using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietiEstate.Shared.Models.Shared;
using DietiEstate.Shared.Models.UserModels;
namespace DietiEstate.Shared.Models.ListingModels;

/// <summary>
/// Represents a listing uploaded by an agent to the system. 
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class Listing
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(Type))]
    public Guid TypeId { get; set; }

    [Required]
    public virtual PropertyType Type { get; set; } = null!;
    
    [Required]
    [Url]
    public string FeaturedImage { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public float Latitude { get; set; }

    [Required]
    public float Longitude { get; set; }

    [Required]
    public decimal Dimensions { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Rooms { get; set; }

    [Required]
    public int Floor { get; set; }

    [Required]
    public bool Available { get; set; } = false;

    [Required]
    public bool Elevator { get; set; } = false;

    [Required]
    [MaxLength(2)]
    public string EnergyClass { get; set; } = string.Empty;

    [Required]
    public int Views { get; set; } = 0;

    [Required]
    [EmailAddress]
    public string OwnerEmail { get; set; } = string.Empty;

    [Required]
    public User? Agent { get; set; } = null;

    [ForeignKey(nameof(Agent))]
    public Guid? AgentUserId { get; set; } = null!;
    
    [Required] 
    public virtual ICollection<Image> ListingImages { get; } = [];
    
    [Required] 
    public virtual ICollection<Service> ListingServices { get; } = [];
    
    [Required] 
    public virtual ICollection<Tag> ListingTags { get; } = [];
}
