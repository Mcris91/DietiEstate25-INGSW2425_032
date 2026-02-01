using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Infrastracture.Extensions;

public static class ListingQueryExtensions
{
    public static IQueryable<Listing> ApplyFilters(this IQueryable<Listing> query, ListingFilterDto filters)
    {
        if (filters.AgentId.HasValue)
            query = query.Where(l => l.AgentUserId == filters.AgentId.Value);
        
        if (!string.IsNullOrEmpty(filters.EnergyClass))
            query = query.Where(l => l.EnergyClass == filters.EnergyClass);
        
        if (filters.TypeId.HasValue)
            query = query.Where(l => l.TypeId == filters.TypeId.Value);
        
        if (filters.Tags?.Count > 0)
            query = query.Where(l => l.ListingTags
                .Any(ls => filters.Tags.Contains(ls.Name)));
            
        return query;
    }

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