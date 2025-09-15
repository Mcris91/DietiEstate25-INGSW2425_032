using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.ListingModels;

/// <summary>
/// Represents the type of <see cref="Listing"/>. 
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
public class Tag
{
    /// <summary>
    /// Gets or sets the unique identifier for the tag.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name for the tag.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of listings associated with the image.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between the service entity and the listing entity,
    /// allowing multiple listings to be associated with a single image.
    /// </remarks>
    public virtual ICollection<ListingModels.Listing> Listings { get; set; } = [];
}