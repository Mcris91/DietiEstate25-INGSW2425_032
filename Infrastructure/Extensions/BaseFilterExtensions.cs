using DietiEstate.Application.Dtos.Filters;

namespace DietiEstate.Infrastructure.Extensions;

public static class BaseFilterExtensions
{
    public static T ApplyRoleFilters<T>(this T filter, string? role, Guid userId, Guid? userAgencyId) where T : BaseFilterDto
    {
        switch (role)
        {
            case "EstateAgent":
                filter.AgentId = userId;
                filter.AgencyId = null;
                break;

            case "SuperAdmin":
            case "SupportAdmin":
                if (userAgencyId == null || userAgencyId == Guid.Empty)
                    throw new UnauthorizedAccessException("Agency ID missing for admin role.");
            
                filter.AgentId = null;
                filter.AgencyId = userAgencyId;
                break;

            case "SystemAdmin":
                filter.AgentId = null;
                filter.AgencyId = null;
                break;

            default:
                throw new UnauthorizedAccessException("User role not authorized.");
        }
        return filter;
    }
}