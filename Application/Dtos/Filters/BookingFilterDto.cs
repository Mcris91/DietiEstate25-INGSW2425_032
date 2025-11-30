using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Enums;
using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Filters;

public class BookingFilterDto
{
    public BookingStatus? Status { get; init; } = BookingStatus.Pending;
    
    public DateTime? DateCreation { get; init; }
    
    public DateTime? DateMeeting { get; init; }
    
    public Guid? ListingId { get; init;}
    
    public Guid? ClientId { get; init;}
    
    public Guid? AgentId { get; init;}

    public string SortOrder { get; init; } = "data";
    
    public string SortBy { get; init; } = "Id";
}