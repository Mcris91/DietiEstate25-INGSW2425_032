using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Requests;

public class BookingRequestDto
{
    public Guid Id { get; set; }
    
    public DateAndTime DateCreated { get; set; }
    
    public DateAndTime DateMeeting { get; set; }

    public bool BookingAccepted { get; set; } = false;
}