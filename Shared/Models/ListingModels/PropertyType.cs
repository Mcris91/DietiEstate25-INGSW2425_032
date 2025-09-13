using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.ListingModels;

/// <summary>
/// Represents the tags associated with a <see cref="Listing"/>. 
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
public class PropertyType
{
    /// <summary>
    /// Gets or sets the unique identifier for the property type.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the icon for the property type.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name for the property type.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}