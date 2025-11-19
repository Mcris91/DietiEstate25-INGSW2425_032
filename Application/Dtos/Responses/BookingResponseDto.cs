namespace DietiEstate.Application.Dtos.Responses;

public class BookingResponseDto
{
    public Guid Id { get; init; }
    
    public required DateTime DateCreation { get; init; }
    
    public DateTime DateMeeting { get; init; }

    public List<string> BookingsForListing { get; init; } = [];
}