namespace DietiEstate.Application.Dtos.Responses;

public class ListingImageResponseDto
{
    public Guid Id { get; init; }
    public required string Url { get; init; }
}