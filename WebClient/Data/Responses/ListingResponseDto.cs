using DietiEstate.WebClient.Data.Common;

namespace DietiEstate.WebClient.Data.Responses;

public class ListingResponseDto
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
    
    public required string FeaturedImage { get; init; }

    public decimal Dimensions { get; init; }

    public decimal Price { get; init; }

    public int Rooms { get; init; }
    
    public int Floor { get; init; }
    
    public required string Address { get; init; }

    public required float Latitude { get; init; }

    public required float Longitude { get; init; }
    public required string City { get; init; }
    
    public required string EnergyClass { get; init; }

    public bool Available { get; init; }
    
    public bool Elevator { get; set; }

    public required ListingTypeDto Type { get; init; }
    
    public UserResponseDto Agent { get; init; }

    public List<ListingImageResponseDto> Images { get; init; } = [];

    public List<ListingServiceDto> Services { get; init; } = [];

    public List<ListingTagDto> Tags { get; init; } = [];
}