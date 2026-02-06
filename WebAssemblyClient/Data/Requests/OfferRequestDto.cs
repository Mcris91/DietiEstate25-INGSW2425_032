using System.ComponentModel.DataAnnotations;

namespace WebAssemblyClient.Data.Requests;

public class OfferRequestDto
{
    [Required(ErrorMessage = "Inserisci un valore per l'offerta")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Inserisci un valore di offerta valido")]
    public decimal Value { get; set; }
    
    public Guid FirstOfferId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public Guid AgentId { get; set; }
    
    public Guid ListingId { get; set; }
}