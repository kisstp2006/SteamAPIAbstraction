namespace SteamOwnershipExample.Core;

public sealed record FriendInfo(
    ulong SteamId,
    string PersonaName,
    UserOnlineState State,
    AppId? CurrentlyPlaying);

public interface IFriendsProvider : IAsyncDisposable
{
    string Name { get; }
    Task<IReadOnlyList<FriendInfo>> ListFriendsAsync(CancellationToken ct = default);
}
