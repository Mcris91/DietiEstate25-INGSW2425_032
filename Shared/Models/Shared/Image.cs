using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.Shared;

/// <summary>
/// Represents an image that can be associated with other entities (e.g. <see cref="ListingModels.Listing"/>). 
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
public class Image
{
    /// <summary>
    /// Gets or sets the unique identifier for the image.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the url for the image.
    /// </summary>
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of listings associated with the image.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between the image entity and the listing entity,
    /// allowing multiple listings to be associated with a single image.
    /// </remarks>
    public virtual ICollection<ListingModels.Listing> Listings { get; set; } = [];
}