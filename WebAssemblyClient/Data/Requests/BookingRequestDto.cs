using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Requests;

public class BookingRequestDto
{
    public Guid Id { get; init;}
   
    public DateTime DateCreation { get; init; }
   
    public DateTime? DateMeeting { get; set; }

    public BookingStatus Status { get; init; } = BookingStatus.Pending;
   
    public Guid ListingId { get; init; }
   
    public Guid AgentId { get; init; }
   
    public Guid ClientId { get; init; }
}