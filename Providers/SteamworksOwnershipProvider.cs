using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksOwnershipProvider : SteamworksProviderBase, IOwnershipProvider
{
    public string Name => "Steamworks SDK";

    public Task<OwnershipResult> CheckAsync(AppId appId, CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();

        var owned = SteamApps.BIsSubscribedApp(new AppId_t(appId.Value));
        return Task.FromResult(new OwnershipResult(appId, owned, Name));
    }
}
