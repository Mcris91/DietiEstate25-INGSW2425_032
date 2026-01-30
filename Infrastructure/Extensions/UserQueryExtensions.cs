using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Infrastracture.Extensions;

public static class UserQueryExtensions
{
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