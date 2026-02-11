using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Requests;

public class BookingFilterDto : BaseFilterDto
{
    public BookingStatus? Status { get; init; }
    
    public DateTime? DateCreation { get; init; }
    
    public DateTime? DateMeeting { get; init; }
    
    public Guid? ListingId { get; init;}
    
    public Guid? ClientId { get; init;}

    public string SortOrder { get; init; } = "desc";
    
    public string SortBy { get; init; } = "data";
}