namespace SteamOwnershipExample.Core;

public enum UserOnlineState
{
    Offline,
    Online,
    Busy,
    Away,
    Snooze,
    LookingToTrade,
    LookingToPlay,
    Invisible
}

public sealed record UserProfile(
    ulong SteamId,
    string PersonaName,
    UserOnlineState State,
    string Language,
    bool IsRunningOnSteamDeck);

public interface IUserInfoProvider : IAsyncDisposable
{
    string Name { get; }
    Task<UserProfile> GetCurrentUserAsync(CancellationToken ct = default);
}
