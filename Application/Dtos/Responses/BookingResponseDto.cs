using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Dtos.Responses;

public class BookingResponseDto
{
    public Guid Id { get; init; }
    
    public string CustomerName { get; init; } = string.Empty;
    
    public string CustomerLastName { get; init; } = string.Empty;
    
    public string CustomerEmail { get; init; } = string.Empty;
    
    public string ListingName { get; init; } = string.Empty;
    
    public DateTime DateCreation { get; init; }
    
    public DateTime DateMeeting { get; init; }

    public BookingStatus Status { get; init; } = BookingStatus.Pending;
    
    public Guid ListingId { get; init; }
   
    public Guid AgentId { get; init; }
   
    public Guid ClientId { get; init; }
}