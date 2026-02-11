using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Entities.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietiEstate.Core.Entities.FavouritesModels;

public class Favourite
{
    [Key]
    public Guid FavouriteId { get; set; }
    
    [Required]
    public User User { get; set; }
    
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    
    [Required]
    public Listing Listing { get; set; }
    
    [ForeignKey(nameof(Listing))]
    public Guid ListingId { get; set; }
    
}