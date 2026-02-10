namespace DietiEstate.Application.Dtos.Responses;

public class UserFavouritesResponseDto
{
    public Guid ListingId { get; set; }
    public string ListingName { get; set; } = string.Empty;
    public decimal ListingPrice { get; set; }
    public string FeaturedImage { get; set; } = string.Empty;
}