using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksUserInfoProvider : SteamworksProviderBase, IUserInfoProvider
{
    public string Name => "Steamworks SDK";

    public Task<UserProfile> GetCurrentUserAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();

        var steamId = SteamUser.GetSteamID();
        var personaName = SteamFriends.GetPersonaName();
        var personaState = SteamFriends.GetPersonaState();
        var language = SteamApps.GetCurrentGameLanguage();
        var onDeck = SteamUtils.IsSteamRunningOnSteamDeck();

        return Task.FromResult(new UserProfile(
            SteamId: steamId.m_SteamID,
            PersonaName: personaName,
            State: MapState(personaState),
            Language: language,
            IsRunningOnSteamDeck: onDeck));
    }

    internal static UserOnlineState MapState(EPersonaState s) => s switch
    {
        EPersonaState.k_EPersonaStateOffline => UserOnlineState.Offline,
        EPersonaState.k_EPersonaStateOnline => UserOnlineState.Online,
        EPersonaState.k_EPersonaStateBusy => UserOnlineState.Busy,
        EPersonaState.k_EPersonaStateAway => UserOnlineState.Away,
        EPersonaState.k_EPersonaStateSnooze => UserOnlineState.Snooze,
        EPersonaState.k_EPersonaStateLookingToTrade => UserOnlineState.LookingToTrade,
        EPersonaState.k_EPersonaStateLookingToPlay => UserOnlineState.LookingToPlay,
        EPersonaState.k_EPersonaStateInvisible => UserOnlineState.Invisible,
        _ => UserOnlineState.Offline
    };
}
