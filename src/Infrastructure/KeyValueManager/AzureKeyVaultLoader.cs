using System.Collections.Concurrent;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using eXtensionSharp;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.KeyValueManager;

public class AzureKeyVaultLoader : IKeyValueLoader, IKeyValueLoopStarter
{
    public ConcurrentDictionary<string, string> Data { get; }
    private Task _loopTask;
    private readonly CancellationToken _cancellationToken = default;
    private readonly ILogger _logger;
    public AzureKeyVaultLoader(ILogger<AzureKeyVaultLoader> logger)
    {
        _logger = logger;
    }
    
    public Task Start(Dictionary<string, string> parameters)
    {
        if (_loopTask.xIsNotEmpty())
        {
            throw new InvalidOperationException("This loader has already been started.");
        }
        
        return StartLoop(parameters["URL"], parameters["SECRET_NAME"]);
    }
    
    private async Task StartLoop(string url, string secretName)
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Azure Credential (로컬 개발에서는 Visual Studio, CLI 인증 또는 Managed Identity 사용 가능)
                var credential = new DefaultAzureCredential();

                // Key Vault 클라이언트 생성
                var client = new SecretClient(new Uri(url), credential);

                // 비밀 가져오기
                KeyVaultSecret retrievedSecret = await client.GetSecretAsync(secretName, cancellationToken: _cancellationToken);
                var map = retrievedSecret.Value.ToDeserialize<Dictionary<string, object>>();
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