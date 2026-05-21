using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksPlayerStatsProvider : SteamworksProviderBase, IPlayerStatsProvider
{
    public string Name => "Steamworks SDK";

    private readonly Callback<UserStatsReceived_t> _statsReceivedCb;
    private bool _statsLoaded;

    public SteamworksPlayerStatsProvider()
    {
        _statsReceivedCb = Callback<UserStatsReceived_t>.Create(OnStatsReceived);
        SteamUserStats.RequestCurrentStats();
    }

    private void OnStatsReceived(UserStatsReceived_t cb)
    {
        if (cb.m_eResult == EResult.k_EResultOK)
            _statsLoaded = true;
    }

    private async Task EnsureStatsLoadedAsync(CancellationToken ct)
    {
        if (_statsLoaded) return;

        var deadline = DateTime.UtcNow.AddSeconds(5);
        while (!_statsLoaded && DateTime.UtcNow < deadline)
        {
            ct.ThrowIfCancellationRequested();
            SteamApiState.RunCallbacks();
            await Task.Delay(50, ct);
        }
        if (!_statsLoaded)
            throw new TimeoutException("Timed out waiting for Steam UserStatsReceived callback.");
    }

    public async Task<AchievementInfo> GetAchievementAsync(string apiName, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);

        SteamUserStats.GetAchievementAndUnlockTime(apiName, out var unlocked, out var unlockTime);
        var displayName = SteamUserStats.GetAchievementDisplayAttribute(apiName, "name");
        var description = SteamUserStats.GetAchievementDisplayAttribute(apiName, "desc");

        DateTime? unlockedAt = unlocked && unlockTime > 0
            ? DateTimeOffset.FromUnixTimeSeconds(unlockTime).UtcDateTime
            : null;

        return new AchievementInfo(
            apiName,
            unlocked,
            unlockedAt,
            string.IsNullOrEmpty(displayName) ? null : displayName,
            string.IsNullOrEmpty(description) ? null : description);
    }

    public async Task<bool> UnlockAchievementAsync(string apiName, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        return SteamUserStats.SetAchievement(apiName);
    }

    public async Task<bool> ClearAchievementAsync(string apiName, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        return SteamUserStats.ClearAchievement(apiName);
    }

    public async Task IndicateAchievementProgressAsync(string apiName, uint current, uint max, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        SteamUserStats.IndicateAchievementProgress(apiName, current, max);
    }

    public async Task<int> GetIntStatAsync(string statName, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        SteamUserStats.GetStat(statName, out int value);
        return value;
    }

    public async Task<float> GetFloatStatAsync(string statName, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        SteamUserStats.GetStat(statName, out float value);
        return value;
    }

    public async Task SetIntStatAsync(string statName, int value, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        SteamUserStats.SetStat(statName, value);
    }

    public async Task SetFloatStatAsync(string statName, float value, CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        SteamUserStats.SetStat(statName, value);
    }

    public async Task<bool> CommitAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        await EnsureStatsLoadedAsync(ct);
        return SteamUserStats.StoreStats();
    }

    public override ValueTask DisposeAsync()
    {
        _statsReceivedCb?.Dispose();
        return base.DisposeAsync();
    }
}
