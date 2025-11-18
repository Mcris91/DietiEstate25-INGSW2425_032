using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Filters;

public class BookingFilterDto
{
    public IReadOnlyList<Guid>? BookingsIds { get; init; }
    
    public DateAndTime? DateCreation { get; init; }
    
    public DateAndTime? DateMeeting { get; init; }
    
}