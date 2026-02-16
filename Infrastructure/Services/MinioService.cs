using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Infrastructure.Config;
using Minio;
using Minio.DataModel.Args;
using SixLabors.ImageSharp;

namespace DietiEstate.Infrastructure.Services;

public class MinioService(IMinioClient minioClient, MinioConfiguration minioConfiguration) : IMinioService
{
    public async Task<string> UploadImageAsync(Stream picture, Guid listingId, Guid imageId)
    {
        var mimeType = IsImage(picture);
        if (mimeType == null) throw new FileFormatException("Il file che stai caricando non è un'immagine!");

        var fileName = $"{listingId}/{imageId}.jpg";

        var args = new PutObjectArgs()
            .WithBucket(minioConfiguration.Bucket)
            .WithObject(fileName)
            .WithStreamData(picture)
            .WithObjectSize(picture.Length)
            .WithContentType(mimeType);
        
            await minioClient.PutObjectAsync(args);
        
        return fileName;
    }

    public async Task<string> GeneratePresignedUrl(string pictureUrl)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(minioConfiguration.Bucket)
            .WithObject(pictureUrl)
            .WithExpiry(3600);

        return await minioClient.PresignedGetObjectAsync(args);
    }

    public string? IsImage(Stream picture)
    {
        try
        {
            using var image = Image.Load(picture);
            picture.Seek(0, SeekOrigin.Begin);
            return image.Metadata.DecodedImageFormat?.DefaultMimeType;
        }
        catch
        {
            picture.Seek(0, SeekOrigin.Begin);
            return null;
        }
    }
}