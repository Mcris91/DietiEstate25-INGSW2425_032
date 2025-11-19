using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Requests;

public class BookingRequestDto
{
   public DateTime DateCreation { get; init; } = DateTime.Now;
   
   public Guid Id { get; init; }
   
   public DateTime DateMeeting { get; init; }

   public Guid ClientId { get; init; }

   public Guid AgentId { get; init; }

   public bool BookingAccepted { get; init; } = false;
   
   public List<Guid>? BookingsForListing { get; init; }
   
}