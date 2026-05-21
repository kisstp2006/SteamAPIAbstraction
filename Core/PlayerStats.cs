namespace SteamOwnershipExample.Core;

public sealed record AchievementInfo(
    string ApiName,
    bool IsUnlocked,
    DateTime? UnlockedAt,
    string? DisplayName,
    string? Description);

public interface IPlayerStatsProvider : IAsyncDisposable
{
    string Name { get; }

    Task<AchievementInfo> GetAchievementAsync(string apiName, CancellationToken ct = default);
    Task<bool> UnlockAchievementAsync(string apiName, CancellationToken ct = default);
    Task<bool> ClearAchievementAsync(string apiName, CancellationToken ct = default);
    Task IndicateAchievementProgressAsync(string apiName, uint current, uint max, CancellationToken ct = default);

    Task<int> GetIntStatAsync(string statName, CancellationToken ct = default);
    Task<float> GetFloatStatAsync(string statName, CancellationToken ct = default);
    Task SetIntStatAsync(string statName, int value, CancellationToken ct = default);
    Task SetFloatStatAsync(string statName, float value, CancellationToken ct = default);

    Task<bool> CommitAsync(CancellationToken ct = default);
}
