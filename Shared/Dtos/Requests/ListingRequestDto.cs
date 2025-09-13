namespace DietiEstate.Shared.Dtos.Requests;

/// <summary>
/// Represents the data transfer object for creating or updating a real estate listing.
/// </summary>
public class ListingRequestDto
{
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Represents the data transfer object for creating or updating a real estate listing.
    /// Contains all necessary properties to define a real estate listing.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier for the type or category of the real estate listing.
    /// Used to classify the listing based on its type (e.g., apartment, house, land).
    /// </summary>
    public Guid TypeId { get; init; }

    /// <summary>
    /// Gets the URL of the featured image associated with the real estate listing.
    /// Represents the primary visual representation of the listing.
    /// </summary>
    public string FeaturedImage { get; init; } = string.Empty;

    /// <summary>
    /// Specifies the address of the real estate property associated with the listing.
    /// Includes any necessary details to locate the property.
    /// </summary>
    public string Address { get; init; } = string.Empty;

    /// <summary>
    /// Represents the latitude coordinate of the real estate listing.
    /// Used to specify the geographical location of the property.
    /// </summary>
    public float Latitude { get; init; }

    /// <summary>
    /// Gets the longitude coordinate of the real estate listing's location.
    /// </summary>
    public float Longitude { get; init; }

    /// <summary>
    /// Represents the dimensions of the real estate property.
    /// Typically measured in square meters or another unit of area.
    /// </summary>
    public decimal Dimensions { get; init; }

    /// <summary>
    /// Represents the price of the real estate listing.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Gets the number of rooms in the real estate listing.
    /// Represents the total count of individual rooms within the property.
    /// </summary>
    public int Rooms { get; init; }

    /// <summary>
    /// Indicates the floor number of the property in the real estate listing.
    /// Used to specify the level of the building where the property is located.
    /// </summary>
    public int Floor { get; init; }

    /// <summary>
    /// Indicates whether the real estate listing is currently available.
    /// </summary>
    public bool Available { get; init; } = false;

    /// <summary>
    /// Indicates whether the property includes an elevator.
    /// </summary>
    public bool Elevator { get; init; } = false;

    /// <summary>
    /// Gets the energy efficiency class of the property, indicating its energy performance level.
    /// </summary>
    public string EnergyClass { get; init; } = string.Empty;

    /// <summary>
    /// Gets the number of views the real estate listing has received.
    /// This property is used to track the popularity or engagement with the listing.
    /// </summary>
    public int Views { get; init; } = 0;

    /// <summary>
    /// Gets the email address of the owner associated with the real estate listing.
    /// </summary>
    /// <remarks>
    /// This property is used to store the contact email of the listing's owner.
    /// It is a required field for communication purposes.
    /// </remarks>
    public string OwnerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the agent user associated with the listing.
    /// This property can be used to link a specific agent to a real estate listing.
    /// </summary>
    public Guid? AgentUserId { get; init; } = null!;

    /// <summary>
    /// Represents the list of service identifiers associated with a real estate listing.
    /// Used to specify the services provided or included in the listing (e.g., cleaning, security).
    /// </summary>
    public List<Guid> Services { get; init; } = [];

    /// <summary>
    /// Represents the collection of tag identifiers associated with the real estate listing.
    /// These tags can be used to categorize or provide additional metadata for the listing.
    /// </summary>
    public List<Guid> Tags { get; init; } = [];

    /// <summary>
    /// Represents a collection of image URLs associated with the real estate listing.
    /// This property stores a list of string values, each representing an image URL.
    /// </summary>
    public List<string> Images { get; init; } = [];
}