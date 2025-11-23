namespace DietiEstate.WebClient.Data.Requests;

public class OfferRequestDto
{
    public decimal Value { get; init; }
    
    public Guid FirstOfferId { get; set; }
    
    public Guid CustomerId { get; init; }
    
    public Guid AgentId { get; init; }
    
    public Guid ListingId { get; init; }
}