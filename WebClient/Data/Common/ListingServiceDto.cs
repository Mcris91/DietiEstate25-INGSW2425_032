namespace DietiEstate.WebClient.Data.Common;

public class ListingServiceDto
{
    public Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Type { get; init; }
    
    public required string Address { get; init; }
    
    public required double Distance { get; init; }
    
    public required double Latitude { get; init; }
    
    public required double Longitude { get; init; }
}