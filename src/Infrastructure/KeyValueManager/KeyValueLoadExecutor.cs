using eXtensionSharp;

namespace Infrastructure.KeyValueManager;

public class KeyValueLoadExecutor
{
    private readonly IKeyValueLoader _loader;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="loader"></param>
    public KeyValueLoadExecutor(IKeyValueLoader loader)
    {
        _loader = loader;
    }

    public void Start(Dictionary<string, string> parameters)
    {
        _loader.xAs<IKeyValueLoopStarter>().Start(parameters);
    }
}