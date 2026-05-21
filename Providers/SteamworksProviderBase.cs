namespace SteamOwnershipExample.Providers;

public abstract class SteamworksProviderBase : IAsyncDisposable
{
    private bool _acquired;

    protected SteamworksProviderBase()
    {
        SteamApiState.Acquire();
        _acquired = true;
    }

    protected void EnsureAcquired()
    {
        if (!_acquired)
            throw new ObjectDisposedException(GetType().Name);
    }

    public virtual ValueTask DisposeAsync()
    {
        if (_acquired)
        {
            SteamApiState.Release();
            _acquired = false;
        }
        return ValueTask.CompletedTask;
    }
}
