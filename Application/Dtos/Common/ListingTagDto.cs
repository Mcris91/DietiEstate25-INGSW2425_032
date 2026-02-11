namespace DietiEstate.Application.Dtos.Common;

public class ListingTagDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    
    public required string Text { get; init; }
}