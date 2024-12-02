using System.Collections.Concurrent;

namespace Infrastructure.KeyValueManager;

public interface IKeyValueLoader
{
    ConcurrentDictionary<string, string> Data { get; }
}

public interface IKeyValueLoopStarter
{
    Task Start(Dictionary<string, string> parameters);
}