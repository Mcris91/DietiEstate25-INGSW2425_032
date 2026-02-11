using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Requests;

public class OfferFilterDto : BaseFilterDto
{
    
    public string CustomerFirstName { get; init; } = string.Empty;
    
    public string CustomerLastName { get; init; } = string.Empty;
    
    public string CustomerEmail { get; init; } = string.Empty;
    public string ListingName { get; init; } = string.Empty;
    public decimal? Value { get; init; }
    public DateTimeOffset Date { get; init; }
    public OfferStatus Status { get; init; }
    public string SortBy { get; init; } = "date";
    public string SortOrder { get; init; } = "desc";
}