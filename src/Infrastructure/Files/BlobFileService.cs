namespace Infrastructure.Files;

public class BlobFileService : IFileService
{
    public Task<bool> Upload(string bucketName, string filePath, string contentType, string[] tags = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> DownloadUrl(string bucketName, string filePath)
    {
        throw new NotImplementedException();
    }

    public Task Download(string bucketName, string filePath, string downloadFilePath)
    {
        throw new NotImplementedException();
    }
}