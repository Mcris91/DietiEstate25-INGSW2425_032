using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Entities.ListingModels;
using Microsoft.VisualBasic;

namespace DietiEstate.Application.Dtos.Filters;

public class BookingFilterDto
{
    public Guid? BookingId { get; init; }
    
    public DateTime? DateCreation { get; init; }
    
    public DateTime? DateMeeting { get; init; }

    public string SortOrder { get; init; } = "desc";
}