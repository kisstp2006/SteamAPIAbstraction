using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksOwnershipProvider : IOwnershipProvider
{
    public string Name => "Steamworks SDK";

    private bool _initialized;

    public SteamworksOwnershipProvider()
    {
        if (!Packsize.Test())
            throw new InvalidOperationException(
                "Steamworks.NET pack-size mismatch — 32/64-bit DLL and build platform are not aligned.");

        if (!DllCheck.Test())
            throw new InvalidOperationException(
                "Steamworks.NET DLL check failed — steam_api64.dll is missing or has the wrong version.");

        if (!SteamAPI.Init())
        {
            throw new InvalidOperationException(
                "SteamAPI.Init() failed. Is the Steam client running? Are you logged in? Is steam_appid.txt present in the working directory?");
        }

        _initialized = true;
    }

    public Task<OwnershipResult> CheckAsync(AppId appId, CancellationToken ct = default)
    {
        if (!_initialized)
            throw new ObjectDisposedException(nameof(SteamworksOwnershipProvider));

        ct.ThrowIfCancellationRequested();

        var owned = SteamApps.BIsSubscribedApp(new AppId_t(appId.Value));
        return Task.FromResult(new OwnershipResult(appId, owned, Name));
    }

    public ValueTask DisposeAsync()
    {
        if (_initialized)
        {
            SteamAPI.Shutdown();
            _initialized = false;
        }
        return ValueTask.CompletedTask;
    }
}
