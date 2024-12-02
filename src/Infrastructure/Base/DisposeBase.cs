namespace Infrastructure.Base;

public class DisposeBase : IDisposable
{
    private volatile bool _disposed;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _disposed = true;
        }
    }

    ~DisposeBase()
    {
        Dispose(false);
    }
}