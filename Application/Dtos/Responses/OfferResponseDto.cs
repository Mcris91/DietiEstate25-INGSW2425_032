using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Dtos.Responses;

public class OfferResponseDto
{
    public Guid Id { get; init; }

    public decimal Value { get; init; }
    
    public DateTimeOffset Date { get; init; }
    
    public OfferStatus Status { get; init; }
    
    public Guid FirstOfferId { get; init; }
    
    public Guid CustomerId { get; init; }
    
    public Guid AgentId { get; init; }
    
    public Guid ListingId { get; init; }
}