using DietiEstate.Core.Enums;
using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Requests;

public class BookingRequestDto
{
   public Guid Id { get; init;}
   
   public DateTime DateCreation { get; init; }
   
   public DateTime DateMeeting { get; init; }

   public BookingStatus Status { get; init; } = BookingStatus.Pending;
   
   public Guid ListingId { get; init; }
   
   public Guid AgentId { get; init; }
   
   public Guid ClientId { get; init; }
}