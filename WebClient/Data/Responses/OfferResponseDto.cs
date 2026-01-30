using DietiEstate.WebClient.Data.Common;

namespace DietiEstate.WebClient.Data.Responses;

public class OfferResponseDto
{
    public Guid Id { get; init; }

    public decimal Value { get; init; }
    
    public DateTimeOffset Date { get; init; }
    
    public OfferStatus Status { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string CustomerLastName { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
    public string ListingName { get; init; } = string.Empty;
    public float ListingPrice { get; init; }
    
    public Guid FirstOfferId { get; init; }
    
    public Guid CustomerId { get; init; }
    
    public Guid AgentId { get; init; }
    
    public Guid ListingId { get; init; }
}