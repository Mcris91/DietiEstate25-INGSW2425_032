namespace WebAssemblyClient.Data.Responses;

public class UserFavouritesResponseDto
{
    public Guid ListingId { get; set; }
    
    public required string ListingName { get; set; }
    
    public required string ListingAddress { get; set; }
    
    public decimal ListingPrice { get; set; }
    
    public required string ListingFeaturedImage { get; set; }
    
    public required string ListingEnergyClass { get; init; }
    
    public float ListingDimensions { get; init; }
    
    public int ListingRooms { get; init; }
    
    public required List<string> ListingTags { get; set; }
}