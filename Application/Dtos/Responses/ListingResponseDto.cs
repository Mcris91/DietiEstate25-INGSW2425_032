using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Application.Dtos.Responses;

public class ListingResponseDto
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string FeaturedImage { get; init; }

    public float Dimensions { get; init; }

    public float Price { get; init; }

    public int Rooms { get; init; }

    public bool Available { get; init; }

    public required string Type { get; init; }

    public List<string> Images { get; init; } = [];

    public List<string> Services { get; init; } = [];

    public List<string> Tags { get; init; } = [];
}