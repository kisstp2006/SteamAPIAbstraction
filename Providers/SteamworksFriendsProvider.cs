using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksFriendsProvider : SteamworksProviderBase, IFriendsProvider
{
    public string Name => "Steamworks SDK";

    public Task<IReadOnlyList<FriendInfo>> ListFriendsAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();

        var count = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        var list = new List<FriendInfo>(count);

        for (var i = 0; i < count; i++)
        {
            var fid = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            var name = SteamFriends.GetFriendPersonaName(fid);
            var state = SteamFriends.GetFriendPersonaState(fid);

            AppId? playing = null;
            if (SteamFriends.GetFriendGamePlayed(fid, out var gameInfo) && gameInfo.m_gameID.IsValid())
                playing = new AppId(gameInfo.m_gameID.AppID().m_AppId);

            list.Add(new FriendInfo(
                SteamId: fid.m_SteamID,
                PersonaName: name,
                State: SteamworksUserInfoProvider.MapState(state),
                CurrentlyPlaying: playing));
        }

        return Task.FromResult<IReadOnlyList<FriendInfo>>(list);
    }
}
