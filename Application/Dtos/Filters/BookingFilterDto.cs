using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Enums;
using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Filters;

public class BookingFilterDto
{
    public BookingStatus? Status { get; init; }
    
    public DateTime? DateCreation { get; init; }
    
    public DateTime? DateMeeting { get; init; }
    
    public Guid? ListingId { get; init;}
    
    public Guid? ClientId { get; init;}
    
    public Guid? AgentId { get; set;}
    
    public Guid? AgencyId { get; set; }

    public string SortOrder { get; init; } = "desc";
    
    public string SortBy { get; init; } = "data";
}