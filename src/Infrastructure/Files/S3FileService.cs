using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using eXtensionSharp;

namespace Infrastructure.Files;

public class S3FileService : IFileService
{
    private readonly IAmazonS3 _client;
    public S3FileService(IAmazonS3 client)
    {
        _client = client;
    }

    public async Task<bool> Upload(string bucketName, string filePath, string contentType, string[] tags = null)
    {
        // PutObjectRequest 생성
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = filePath,
            FilePath = filePath,
            ContentType = contentType,
        };

        putRequest.TagSet = new()
        {
            new() { Key = "filename", Value = Path.GetFileName(filePath) },
        };

        if (tags.xIsNotEmpty())
        {
            foreach (var tag in tags)
            {
                putRequest.TagSet.Add(new() { Key = tag, Value = tag });
            }
        }
            
        putRequest.Metadata.Add("Content-Disposition", $"attachment; filename*=UTF-8''\"{WebUtility.UrlEncode(filePath).Replace("+", " ")}\"");

        // 파일 업로드 실행
        PutObjectResponse response = await _client.PutObjectAsync(putRequest);

        return true;
    }

    public async Task<string> DownloadUrl(string bucketName, string filePath)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = filePath,
            Expires = DateTime.UtcNow.AddMinutes(10),
            ResponseHeaderOverrides = new ResponseHeaderOverrides
            {
                ContentDisposition = $"attachment; filename*=UTF-8''\"{WebUtility.UrlEncode(filePath).Replace("+", " ")}\""
            }  
        };

        return await _client.GetPreSignedURLAsync(request);
    }
    
    public async Task Download(string bucketName, string filePath, string downloadFilePath)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = filePath
        };

        using GetObjectResponse response = await _client.GetObjectAsync(request);
        await using Stream responseStream = response.ResponseStream;
        await using FileStream fileStream = new FileStream(downloadFilePath, FileMode.Create, FileAccess.Write);
        await responseStream.CopyToAsync(fileStream);
    }    
}

