namespace DietiEstate.Infrastructure.Config;

public class MinioConfiguration(string endpoint, 
    string accessKey, 
    string secretKey, 
    string bucket)
{
    public string Endpoint { get; } = endpoint;
    public string AccessKey { get; } = accessKey;
    public string SecretKey { get; } = secretKey;
    public string Bucket { get; } = bucket;
}