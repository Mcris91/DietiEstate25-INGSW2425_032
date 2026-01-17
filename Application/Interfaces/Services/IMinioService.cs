namespace DietiEstate.Application.Interfaces.Services;

public interface IMinioService
{
    Task<string> UploadImageAsync(Stream picture, Guid listingId, Guid imageId);

    public string? IsImage(Stream picture);

    Task<string> GeneratePresignedUrl(string pictureUrl);
}