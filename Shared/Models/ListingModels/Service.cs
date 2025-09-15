using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.ListingModels;

/// <summary>
/// Represents the services associated with a <see cref="Listing"/>. 
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
public class Service
{
    /// <summary>
    /// Gets or sets the unique identifier for the service.
    /// </summary>
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Gets or sets the name for the service.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of listings associated with the image.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between the image entity and the listing entity,
    /// allowing multiple listings to be associated with a single image.
    /// </remarks>
    public virtual ICollection<ListingModels.Listing> Listings { get; set; } = [];
}