namespace WebAssemblyClient.Data.Requests;

public class ListingImageRequestDto
{
    public Guid? Id { get; set; }
    public byte[] Image { get; set; } = [];
    
    public string PreviewUrl { get; set; } = string.Empty;
}