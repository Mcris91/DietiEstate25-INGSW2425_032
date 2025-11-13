namespace DietiEstate.Application.Dtos.Responses;

public class PropertyTypeResponseDto
{
    public required Guid Id { get; set; }

    public required string Icon { get; set; }

    public required string Name { get; set; }
    
    public required string Code { get; set; }
}