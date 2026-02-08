using System.ComponentModel.DataAnnotations;
using DietiEstate.Application.Dtos.Common;

namespace DietiEstate.Application.Dtos.Requests;

public class ListingRequestDto
{
    public string Name { get; init; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; init; } = string.Empty;

    public byte[] FeaturedImage { get; init; } = [];

    public string Address { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public float Latitude { get; init; }

    public float Longitude { get; init; }

    public decimal Dimensions { get; init; }

    public decimal Price { get; init; }

    public int Rooms { get; init; }

    public int Floor { get; init; }

    public bool Available { get; init; } = true;

    public bool Elevator { get; init; } = false;

    public string EnergyClass { get; init; } = string.Empty;

    public int Views { get; init; } = 0;

    public string OwnerEmail { get; init; } = string.Empty;

    public Guid? AgentUserId { get; init; } = null;
    
    public string TypeCode { get; init; } = string.Empty;
    
    public List<ListingImageRequestDto> Images { get; init; } = [];

    public List<ListingServiceDto> Services { get; init; } = [];

    public List<ListingTagDto> Tags { get; init; } = [];
}
