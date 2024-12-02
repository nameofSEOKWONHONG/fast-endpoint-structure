using System.Collections.Concurrent;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using eXtensionSharp;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.KeyValueManager;

public class AwsSecretManagerLoader : IKeyValueLoader, IKeyValueLoopStarter
{
    public ConcurrentDictionary<string, string> Data { get; } = new();
    private Task _loopTask;
    private readonly CancellationToken _cancellationToken = default;
    private readonly ILogger _logger;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    public AwsSecretManagerLoader(ILogger<AwsSecretManagerLoader> logger)
    {
        _logger = logger;
    }

    public Task Start(Dictionary<string, string> parameters)
    {
        if (_loopTask.xIsNotEmpty())
        {
            throw new InvalidOperationException("This loader has already been started.");
        }
        
        _loopTask = StartLoop(parameters["KEY_ID"]
            , parameters["ACCESS_KEY"]
            , parameters["REGION"]
            , parameters["SECRET_NAME"]
            ,parameters["VERSION_STAGE"]);

        return _loopTask;
    }
    
    private async Task StartLoop(string keyId, string accessKey, string region, string secretName, string versionStage)
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = new AmazonSecretsManagerClient(keyId, accessKey, RegionEndpoint.GetBySystemName(region));
                var request = new GetSecretValueRequest()
                {
                    SecretId = secretName,
                    VersionStage = versionStage, // VersionStage defaults to AWSCURRENT if unspecified.
                };

                var response = await client.GetSecretValueAsync(request, _cancellationToken);
                var map = response.SecretString.ToDeserialize<Dictionary<string, Object>>();
                foreach (var keyValuePair in map)
                {
                    Data.AddOrUpdate(
                        keyValuePair.Key,
                        // 키가 없을 경우 새 ConcurrentDictionary를 생성
                        key => keyValuePair.Value.ToString(),
                        // 키가 이미 존재할 경우 업데이트 로직
                        (key, oldValue) => keyValuePair.Value.ToString());
                }
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e, "An operation was canceled.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected exception was thrown.");
                throw;
            }
            
            await Task.Delay(5000, _cancellationToken);  
        }
    }
}