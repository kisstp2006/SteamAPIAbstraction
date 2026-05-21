using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksOverlayController : SteamworksProviderBase, IOverlayController
{
    public string Name => "Steamworks SDK";

    public bool IsOverlayEnabled
    {
        get
        {
            EnsureAcquired();
            return SteamUtils.IsOverlayEnabled();
        }
    }

    public Task ActivateAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamFriends.ActivateGameOverlay("");
        return Task.CompletedTask;
    }

    public Task ActivatePageAsync(OverlayPage page, CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamFriends.ActivateGameOverlay(MapPage(page));
        return Task.CompletedTask;
    }

    public Task ActivateProfileAsync(ulong steamId, CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamFriends.ActivateGameOverlayToUser("steamid", new CSteamID(steamId));
        return Task.CompletedTask;
    }

    public Task ActivateWebPageAsync(string url, CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamFriends.ActivateGameOverlayToWebPage(url);
        return Task.CompletedTask;
    }

    public Task SetRichPresenceAsync(string key, string? value, CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamFriends.SetRichPresence(key, value);
        return Task.CompletedTask;
    }

    public Task ClearRichPresenceAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamFriends.ClearRichPresence();
        return Task.CompletedTask;
    }

    public Task TriggerScreenshotAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        SteamScreenshots.TriggerScreenshot();
        return Task.CompletedTask;
    }

    private static string MapPage(OverlayPage page) => page switch
    {
        OverlayPage.Friends => "Friends",
        OverlayPage.Community => "Community",
        OverlayPage.Players => "Players",
        OverlayPage.Settings => "Settings",
        OverlayPage.OfficialGameGroup => "OfficialGameGroup",
        OverlayPage.Stats => "Stats",
        OverlayPage.Achievements => "Achievements",
        _ => ""
    };
}
