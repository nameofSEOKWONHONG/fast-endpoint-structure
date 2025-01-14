namespace Infrastructure.Files;

public interface IFileService
{
    Task<bool> Upload(string bucketName, string filePath, string contentType, string[] tags = null);
    Task<string> DownloadUrl(string bucketName, string filePath);
    Task Download(string bucketName, string filePath, string downloadFilePath);
}