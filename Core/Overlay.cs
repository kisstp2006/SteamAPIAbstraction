namespace SteamOwnershipExample.Core;

public enum OverlayPage
{
    Friends,
    Community,
    Players,
    Settings,
    OfficialGameGroup,
    Stats,
    Achievements
}

public interface IOverlayController : IAsyncDisposable
{
    string Name { get; }
    bool IsOverlayEnabled { get; }

    Task ActivateAsync(CancellationToken ct = default);
    Task ActivatePageAsync(OverlayPage page, CancellationToken ct = default);
    Task ActivateProfileAsync(ulong steamId, CancellationToken ct = default);
    Task ActivateWebPageAsync(string url, CancellationToken ct = default);
    Task SetRichPresenceAsync(string key, string? value, CancellationToken ct = default);
    Task ClearRichPresenceAsync(CancellationToken ct = default);
    Task TriggerScreenshotAsync(CancellationToken ct = default);
}
