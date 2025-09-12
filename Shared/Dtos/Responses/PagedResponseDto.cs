namespace DietiEstate.Shared.Dtos.Responses;

/// <summary>
/// Represents a paginated response containing a collection of items with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items contained in the response.</typeparam>
public record PagedResponseDto<T>
{
    /// <summary>
    /// Gets the number of items per page. Null if pagination is not applied.
    /// </summary>
    public int? PageSize { get; init; }

    /// <summary>
    /// Gets the current page number (1-based). Null if pagination is not applied.
    /// </summary>
    public int? PageNumber { get; init; }

    /// <summary>
    /// Gets the previous page number. Null if this is the first page or pagination is not applied.
    /// </summary>
    public int? PrevPageNumber { get; init; }

    /// <summary>
    /// Gets the next page number. Null if this is the last page or pagination is not applied.
    /// </summary>
    public int? NextPageNumber { get; init; }

    /// <summary>
    /// Gets the total number of pages available. Null if pagination is not applied.
    /// </summary>
    public int? TotalPages { get; init; }

    /// <summary>
    /// Gets the collection of items for the current page or all items if pagination is not applied.
    /// </summary>
    public IEnumerable<T> Items { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResponseDto{T}"/> class.
    /// </summary>
    /// <param name="query">The source query containing <see cref="BaseModel"/> items to be mapped and paginated.</param>
    /// <param name="responsePageSize">The number of items per page. If null, pagination is not applied.</param>
    /// <param name="responsePageNumber">The requested page number (1-based). If null, pagination is not applied.</param>
    /// <remarks>
    /// If both <paramref name="responsePageSize"/> and <paramref name="responsePageNumber"/> are provided,
    /// the response will be paginated with calculated pagination metadata. Otherwise, all items will be returned
    /// without pagination metadata.
    /// </remarks>
    public PagedResponseDto(
        IEnumerable<T> query,
        int? responsePageSize = null,
        int? responsePageNumber = null)
    {
        var listQuery = query.ToList();
        PageSize = responsePageSize;
        PageNumber = responsePageNumber;
        if (PageNumber.HasValue && PageSize.HasValue)
        {
            Items = listQuery.Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);
            PrevPageNumber = PageNumber <= 1 ? null : PageNumber - 1;
            TotalPages = (int)Math.Ceiling((double)listQuery.Count / PageSize.Value);
            NextPageNumber = PageNumber >= TotalPages ? null : PageNumber + 1;
        }
        else
        {
            Items = listQuery;
        }
    }
}