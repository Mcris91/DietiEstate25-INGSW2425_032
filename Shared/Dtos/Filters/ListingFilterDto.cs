namespace DietiEstate.Shared.Dtos.Filters;

/// <summary>
/// Data transfer object containing filter parameters for listing queries.
/// Used to specify criteria for filtering, sorting, and retrieving property listings.
/// </summary>
public class ListingFilterDto
{
    /// <summary>
    /// Gets or sets the property type identifier to filter listings by specific type.
    /// If null, no type filtering is applied.
    /// </summary>
    public Guid? TypeId { get; init; }
    
    /// <summary>
    /// Gets or sets the collection of service identifiers to filter listings that have any of the specified services.
    /// If null or empty, no service filtering is applied.
    /// </summary>
    public IReadOnlyList<Guid>? ServiceIds { get; init; }
    
    /// <summary>
    /// Gets or sets the collection of tag identifiers to filter listings that have any of the specified tags.
    /// If null or empty, no tag filtering is applied.
    /// </summary>
    public IReadOnlyList<Guid>? TagIds { get; init; }
    
    /// <summary>
    /// Gets or sets the minimum price threshold for filtering listings.
    /// Only listings with price greater than or equal to this value will be included.
    /// If null, no minimum price filtering is applied.
    /// </summary>
    public decimal? MinPrice { get; init; }
    
    /// <summary>
    /// Gets or sets the maximum price threshold for filtering listings.
    /// Only listings with price less than or equal to this value will be included.
    /// If null, no maximum price filtering is applied.
    /// </summary>
    public decimal? MaxPrice { get; init; }
    
    /// <summary>
    /// Gets or sets the minimum number of rooms for filtering listings.
    /// Only listings with room count greater than or equal to this value will be included.
    /// If null, no minimum room filtering is applied.
    /// </summary>
    public decimal? MinRooms { get; init; }
    
    /// <summary>
    /// Gets or sets the maximum number of rooms for filtering listings.
    /// Only listings with room count less than or equal to this value will be included.
    /// If null, no maximum room filtering is applied.
    /// </summary>
    public decimal? MaxRooms { get; init; }
    
    /// <summary>
    /// Gets or sets the minimum size/dimensions threshold for filtering listings in square meters.
    /// Only listings with dimensions greater than or equal to this value will be included.
    /// If null, no minimum size filtering is applied.
    /// </summary>
    public decimal? MinSize { get; init; }
    
    /// <summary>
    /// Gets or sets the maximum size/dimensions threshold for filtering listings in square meters.
    /// Only listings with dimensions less than or equal to this value will be included.
    /// If null, no maximum size filtering is applied.
    /// </summary>
    public decimal? MaxSize { get; init; }

    /// <summary>
    /// Gets or sets the field name to sort the results by.
    /// Supported values include: "price", "rooms", "dimensions", "views", "floor".
    /// The default value is "views".
    /// </summary>
    public string SortBy { get; init; } = "views";
    
    /// <summary>
    /// Gets or sets the sort order direction.
    /// Valid values are "asc" for ascending or "desc" for descending order.
    /// The default value is "desc".
    /// </summary>
    public string SortOrder { get; init; } = "desc";
}