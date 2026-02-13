using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Dtos.Filters;

public class OfferFilterDto
{
    public Guid? AgentId { get; set; }
    
    public Guid? AgencyId { get; set; }
    
    public string CustomerFirstName { get; init; } = string.Empty;
    public string CustomerLastName { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
    public string ListingName { get; init; } = string.Empty;
    public decimal? Value { get; init; }
    public DateTimeOffset? Date { get; init; }
    public OfferStatus? Status { get; init; }
    public string SortBy { get; init; } = "date";
    public string SortOrder { get; init; } = "desc";
}