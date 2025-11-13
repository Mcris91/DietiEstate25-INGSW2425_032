using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Core.Entities.BookingModels;

public class Booking
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
}