using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using DietiEstate.Core.Constants;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Core.Entities.BookingModels;

public class Booking
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public DateTime DateCreation { get; set; }
    
    [Required]
    public DateTime DateMeeting { get; set; }
    
    [Required]
    public bool BookingAccepted { get; set; }
    
    [ForeignKey(nameof(Listing))]
    public Guid ListingId { get; set; }
    
    [Required]
    public User? Agent{get;set;}
    
    [ForeignKey(nameof(Agent))]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public User? Client { get; set; }
    
    [ForeignKey(nameof(Client))]
    [EmailAddress]
    public string? ClientEmail { get; set; }
    
}