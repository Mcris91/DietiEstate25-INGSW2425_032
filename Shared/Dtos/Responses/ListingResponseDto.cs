namespace DietiEstate.Shared.Dtos.Responses;

/// <summary>
/// Represents the response data transfer object for a listing in the system.
/// </summary>
/// <remarks>
/// This class is used to encapsulate the details of a property listing in a format that can be returned to the client.
/// It contains various attributes of the listing, such as its identifier, basic details, images, services, tags,
/// and availability status.
/// </remarks>
public class ListingResponseDto
{
    /// <summary>
    /// Gets the unique identifier of the property listing.
    /// </summary>
    /// <remarks>
    /// The identifier is used to uniquely distinguish each property listing in the system.
    /// It is represented as a globally unique identifier (GUID).
    /// </remarks>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the property listing.
    /// </summary>
    /// <remarks>
    /// The name is used to provide a descriptive title for the property, making it easily identifiable
    /// in search results or listing details. This value is mandatory and typically represents a human-readable
    /// label or identifier for the property.
    /// </remarks>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the primary image of the property listing.
    /// </summary>
    /// <remarks>
    /// This image serves as the featured representation of the property.
    /// It is typically displayed prominently in listing previews or summaries.
    /// </remarks>
    public required string FeaturedImage { get; init; }

    /// <summary>
    /// Gets the dimensions of the property associated with the listing.
    /// </summary>
    /// <remarks>
    /// Represents the size or scale of the property, typically measured in square meters or another unit of area.
    /// This value provides a key attribute for understanding the property's physical space.
    /// </remarks>
    public float Dimensions { get; init; }

    /// <summary>
    /// Gets the price of the property listing.
    /// </summary>
    /// <remarks>
    /// The price represents the monetary value of the property listing.
    /// It is typically expressed as a floating-point number to accommodate various pricing structures, including decimals.
    /// </remarks>
    public float Price { get; init; }

    /// <summary>
    /// Gets the number of rooms available in the property listing.
    /// </summary>
    /// <remarks>
    /// The rooms count indicates the total number of individual rooms in the property.
    /// This value is used to provide potential buyers or renters with an understanding
    /// of the size and capacity of the property.
    /// </remarks>
    public int Rooms { get; init; }

    /// <summary>
    /// Indicates whether the property listing is currently available.
    /// </summary>
    /// <remarks>
    /// This property specifies the availability status of the listing.
    /// It is used to determine whether the property is open for reservations or inquiries.
    /// </remarks>
    public bool Available { get; init; }

    /// <summary>
    /// Gets the type of the property listing.
    /// </summary>
    /// <remarks>
    /// The type indicates the classification or category of the property, such as residential, commercial, or industrial.
    /// It is derived from the associated property type model and represented as a string.
    /// </remarks>
    public required string Type { get; init; }

    /// <summary>
    /// Gets or sets the collection of image URLs associated with the listing.
    /// </summary>
    /// <remarks>
    /// This property contains a list of strings, where each string represents the URL of an image for the listing.
    /// It is used to provide visual representation or gallery of the property.
    /// </remarks>
    public List<string> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of services associated with the property listing.
    /// </summary>
    /// <remarks>
    /// Each service represents a feature or amenity available for the property listing.
    /// The services are stored as a collection of strings, where each string represents
    /// the name of a service (e.g., "Wi-Fi", "Parking", "Pool").
    /// </remarks>
    public List<string> Services { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of tags associated with the property listing.
    /// </summary>
    /// <remarks>
    /// Tags represent keywords or labels used to categorize or describe the property listing.
    /// They provide additional context about the listing, such as its special features,
    /// location-specific attributes, or target audience.
    /// </remarks>
    public List<string> Tags { get; set; } = [];
}