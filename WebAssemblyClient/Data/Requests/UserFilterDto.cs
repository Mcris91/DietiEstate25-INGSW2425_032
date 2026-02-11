using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Requests;

public class UserFilterDto : BaseFilterDto
{
    public Guid? AgencyId { get; init;}
    
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public UserRole? Role { get; init; }

    public string SortBy { get; init; } = "firstName";

    public string SortOrder { get; init; } = "desc";
}