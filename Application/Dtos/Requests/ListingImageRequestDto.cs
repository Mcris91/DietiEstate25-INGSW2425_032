namespace DietiEstate.Application.Dtos.Requests;

public class ListingImageRequestDto
{
    public Guid? Id { get; set; }
    public required byte[] Image { get; set; } = [];
    
    public string PreviewUrl { get; set; } = string.Empty;
}