namespace DietiEstate.WebClient.Data.Requests;

public class OfferRequestDto
{
    public decimal Value { get; set; }
    
    public Guid FirstOfferId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public Guid AgentId { get; set; }
    
    public Guid ListingId { get; set; }
}