using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietiEstate.Shared.Models.Shared;
using DietiEstate.Shared.Models.UserModels;
namespace DietiEstate.Shared.Models.ListingModels;

/// <summary>
/// Represents a listing uploaded by a <see cref="UserModels.UserRole.Agent"/> to the system. 
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class Listing
{
    /// <summary>
    /// Gets or sets the unique identifier for the listing.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name/title of the property listing.
    /// Maximum length: 50 characters.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the property.
    /// Maximum length: 5000 characters.
    /// </summary>
    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key referencing the property type.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Type))]
    public Guid TypeId { get; set; }

    /// <summary>
    /// Gets or sets the property type (e.g., apartment, house, commercial).
    /// Navigation property for TypeId.
    /// </summary>
    [Required]
    public virtual PropertyType Type { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the URL of the main featured image for the listing.
    /// Must be a valid URL format.
    /// </summary>
    [Required]
    [Url]
    public string FeaturedImage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full address of the property.
    /// </summary>
    [Required]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the latitude coordinate of the property location.
    /// </summary>
    [Required]
    public float Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the property location.
    /// </summary>
    [Required]
    public float Longitude { get; set; }

    /// <summary>
    /// Gets or sets the total area/dimensions of the property in square meters.
    /// </summary>
    [Required]
    public decimal Dimensions { get; set; }

    /// <summary>
    /// Gets or sets the price of the property listing.
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the number of rooms in the property.
    /// </summary>
    [Required]
    public int Rooms { get; set; }

    /// <summary>
    /// Gets or sets the floor number where the property is located.
    /// </summary>
    [Required]
    public int Floor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the property is currently available for sale/rent.
    /// Default value is false.
    /// </summary>
    [Required]
    public bool Available { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the building has an elevator.
    /// Default value is false.
    /// </summary>
    [Required]
    public bool Elevator { get; set; } = false;

    /// <summary>
    /// Gets or sets the energy efficiency class of the property (e.g., A+, B, C).
    /// Maximum length: 2 characters.
    /// </summary>
    [Required]
    [MaxLength(2)]
    public string EnergyClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times this listing has been viewed.
    /// Default value is 0.
    /// </summary>
    [Required]
    public int Views { get; set; } = 0;

    /// <summary>
    /// Gets or sets the email address of the property owner.
    /// Must be a valid email format.
    /// </summary>
    [Required]
    [EmailAddress]
    public string OwnerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the real estate agent assigned to this listing.
    /// Navigation property for AgentUserId. Can be null if no agent is assigned.
    /// </summary>
    [Required]
    public User? Agent { get; set; } = null;

    /// <summary>
    /// Gets or sets the foreign key referencing the assigned agent user.
    /// Can be null if no agent is assigned to this listing.
    /// </summary>
    [ForeignKey(nameof(Agent))]
    public Guid? AgentUserId { get; set; } = null!;
    
    /// <summary>
    /// Gets the collection of images associated with this listing.
    /// Represents a many-to-many relationship with Image entities.
    /// </summary>
    [Required] 
    public virtual ICollection<Image> ListingImages { get; } = [];
    
    /// <summary>
    /// Gets the collection of services/amenities available for this listing.
    /// Represents a many-to-many relationship with Service entities.
    /// </summary>
    [Required] 
    public virtual ICollection<Service> ListingServices { get; } = [];
    
    /// <summary>
    /// Gets the collection of tags associated with this listing for categorization and filtering.
    /// Represents a many-to-many relationship with Tag entities.
    /// </summary>
    [Required] 
    public virtual ICollection<Tag> ListingTags { get; } = [];
}
