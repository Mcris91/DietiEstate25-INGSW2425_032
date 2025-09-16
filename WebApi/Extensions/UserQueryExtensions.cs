using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Extensions;

/// <summary>
/// Provides extension methods for querying and manipulating <see cref="User"/> collections.
/// </summary>
public static class UserQueryExtensions
{
    /// <summary>
    /// Applies filtering criteria to an <see cref="IQueryable{User}"/> based on the specified <see cref="UserFilterDto"/> object.
    /// </summary>
    /// <param name="query">The initial <see cref="IQueryable{User}"/> to which filters will be applied.</param>
    /// <param name="filters">
    /// An instance of <see cref="UserFilterDto"/> containing the filtering conditions, such as
    /// <see cref="UserFilterDto.FirstName"/>, <see cref="UserFilterDto.LastName"/>, <see cref="UserFilterDto.Email"/>, and <see cref="UserFilterDto.Role"/>.
    /// </param>
    /// <returns>An <see cref="IQueryable{User}"/> modified with the specified filtering criteria.</returns>
    public static IQueryable<User> ApplyFilters(this IQueryable<User> query, UserFilterDto filters)
    {
        if (!string.IsNullOrEmpty(filters.FirstName))
            query = query.Where(u =>
                u.FirstName.Contains(filters.FirstName, StringComparison.CurrentCultureIgnoreCase));
        if (!string.IsNullOrEmpty(filters.LastName))
            query = query.Where(u => u.LastName.Contains(filters.LastName, StringComparison.CurrentCultureIgnoreCase));
        if (!string.IsNullOrEmpty(filters.Email))
            query = query.Where(u => u.Email.Contains(filters.Email, StringComparison.CurrentCultureIgnoreCase));
        if (filters.Role.HasValue)
            query = query.Where(u => u.Role == filters.Role);
        
        return query;       
    }

    /// <summary>
    /// Applies sorting criteria to an <see cref="IQueryable{User}"/> based on the specified sortBy and sortOrder parameters.
    /// </summary>
    /// <param name="query">The initial <see cref="IQueryable{User}"/> to which sorting will be applied.</param>
    /// <param name="sortBy">
    /// A string representing the property by which to sort, such as "firstName", "lastName", "email", or "role".
    /// If an unrecognized property is provided, the query defaults to sorting by "firstName".
    /// </param>
    /// <param name="sortOrder">
    /// A string indicating the sorting order, either "asc" for ascending or "desc" for descending.
    /// If an unrecognized value is provided, the query defaults to ascending order.
    /// </param>
    /// <returns>
    /// An <see cref="IQueryable{User}"/> modified with the specified sorting criteria.
    /// </returns>
    public static IQueryable<User> ApplySorting(this IQueryable<User> query, string sortBy, string sortOrder)
    {
        return sortBy.ToLower() switch
        {
            "firstName" => sortOrder == "desc" ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
            "lastName" => sortOrder == "desc" ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
            "email" => sortOrder == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "role" => sortOrder == "desc" ? query.OrderByDescending(u => u.Role) : query.OrderBy(u => u.Role),
            _ => query.OrderBy(u => u.FirstName)
        };
    }
}