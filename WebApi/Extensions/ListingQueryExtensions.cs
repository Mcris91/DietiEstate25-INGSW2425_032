using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.WebApi.Extensions;

/// <summary>
/// Provides extension methods to query and filter listings.
/// </summary>
public static class ListingQueryExtensions
{
    /// <summary>
    /// Applies the specified filters to an <see cref="IQueryable{T}"/> of <see cref="Listing"/> objects.
    /// </summary>
    /// <param name="query">
    /// The queryable collection of <see cref="Listing"/> objects to which the filters will be applied.
    /// </param>
    /// <param name="filters">
    /// An instance of <see cref="ListingFilterDto"/> containing the filters to be applied,
    /// such as TypeId, ServiceIds, or TagIds.
    /// </param>
    /// <returns>
    /// A filtered <see cref="IQueryable{T}"/> collection based on the provided <see cref="ListingFilterDto"/>.
    /// </returns>
    public static IQueryable<Listing> ApplyFilters(this IQueryable<Listing> query, ListingFilterDto filters)
    {
        if (filters.TypeId.HasValue)
            query = query.Where(l => l.TypeId == filters.TypeId.Value);

        if (filters.ServiceIds?.Count > 0)
            query = query.Where(l => l.ListingServices
                .Any(ls => filters.ServiceIds.Contains(ls.Id)));
        
        if (filters.TagIds?.Count > 0)
            query = query.Where(l => l.ListingTags
                .Any(ls => filters.TagIds.Contains(ls.Id)));
            
        return query;
    }

    /// <summary>
    /// Applies numeric filters, such as price, room count, and size, to an <see cref="IQueryable{T}"/> of <see cref="Listing"/> objects.
    /// </summary>
    /// <param name="query">
    /// The queryable collection of <see cref="Listing"/> objects to which the numeric filters will be applied.
    /// </param>
    /// <param name="filters">
    /// An instance of <see cref="ListingFilterDto"/> containing the numeric filter criteria,
    /// such as MinPrice, MaxPrice, MinRooms, MaxRooms, MinSize, and MaxSize.
    /// </param>
    /// <returns>
    /// A filtered <see cref="IQueryable{T}"/> collection based on the provided numeric criteria in <see cref="ListingFilterDto"/>.
    /// </returns>
    public static IQueryable<Listing> ApplyNumericFilters(this IQueryable<Listing> query, ListingFilterDto filters)
    {
        if (filters.MinPrice.HasValue)
            query = query.Where(l => l.Price >= filters.MinPrice.Value);
        if (filters.MaxPrice.HasValue)
            query = query.Where(l => l.Price <= filters.MaxPrice.Value);
        
        if (filters.MinRooms.HasValue)
            query = query.Where(l => l.Rooms >= filters.MinRooms.Value);
        if (filters.MaxRooms.HasValue)
            query = query.Where(l => l.Rooms <= filters.MaxRooms.Value);
        
        if (filters.MinSize.HasValue)
            query = query.Where(l => l.Dimensions >= filters.MinSize.Value);
        if (filters.MaxSize.HasValue)
            query = query.Where(l => l.Dimensions <= filters.MaxSize.Value);

        return query;
    }

    /// <summary>
    /// Applies sorting to an <see cref="IQueryable{T}"/> of <see cref="Listing"/> objects based on the specified field and sort order.
    /// </summary>
    /// <param name="query">
    /// The queryable collection of <see cref="Listing"/> objects to which the sorting will be applied.
    /// </param>
    /// <param name="sortBy">
    /// The field name by which to sort the collection (e.g., "price", "rooms", "dimensions", "views", "floor").
    /// </param>
    /// <param name="sortOrder">
    /// The sort order to apply, either "asc" for ascending or "desc" for descending. Defaults to ascending if not provided.
    /// </param>
    /// <returns>
    /// A sorted <see cref="IQueryable{T}"/> collection based on the specified field and sort order.
    /// </returns>
    public static IQueryable<Listing> ApplySorting(this IQueryable<Listing> query, string sortBy, string sortOrder)
    {
        return sortBy.ToLower() switch
        {
            "price" => sortOrder == "desc" ? query.OrderByDescending(l => l.Price) : query.OrderBy(l => l.Price),
            "rooms" => sortOrder == "desc" ? query.OrderByDescending(l => l.Rooms) : query.OrderBy(l => l.Rooms),
            "dimensions" => sortOrder == "desc" ? query.OrderByDescending(l => l.Dimensions) : query.OrderBy(l => l.Dimensions),
            "views" => sortOrder == "desc" ? query.OrderByDescending(l => l.Views) : query.OrderBy(l => l.Views),
            "floor" => sortOrder == "desc" ? query.OrderByDescending(l => l.Floor) : query.OrderBy(l => l.Floor),
            _ => query.OrderBy(l => l.Views)
        };
    }
}