using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.Shared.Dtos.Responses;

/// <summary>
/// Represents a response data transfer object for a <see cref="Listing"/> entity.
/// </summary>
public class ListingResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the property listing.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets or sets the name of the property listing.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the primary image of the property listing.
    /// </summary>
    public required string FeaturedImage { get; init; }

    /// <summary>
    /// Gets or sets the dimensions of the property associated with the listing.
    /// </summary>
    public float Dimensions { get; init; }

    /// <summary>
    /// Gets or sets the price of the property listing.
    /// </summary>
    public float Price { get; init; }

    /// <summary>
    /// Gets or sets the number of rooms available in the property listing.
    /// </summary>
    public int Rooms { get; init; }

    /// <summary>
    /// Indicates whether the property listing is currently available.
    /// </summary>
    public bool Available { get; init; }

    /// <summary>
    /// Gets or sets the type of the property listing.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Gets or sets the collection of image URLs associated with the listing.
    /// </summary>
    public List<string> Images { get; init; } = [];

    /// <summary>
    /// Gets or sets the list of services associated with the property listing.
    /// </summary>
    public List<string> Services { get; init; } = [];

    /// <summary>
    /// Gets or sets the collection of tags associated with the property listing.
    /// </summary>
    public List<string> Tags { get; init; } = [];
}