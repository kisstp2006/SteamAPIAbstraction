using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample06_UserInfo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[6] User info");
        Console.WriteLine("    Synchronous reads from SteamUser, SteamFriends, SteamApps, SteamUtils.");
        Console.WriteLine();

        await using IUserInfoProvider users = new SteamworksUserInfoProvider();

        var p = await users.GetCurrentUserAsync();

        Console.WriteLine($"    SteamID64    : {p.SteamId}");
        Console.WriteLine($"    Persona name : {p.PersonaName}");
        Console.WriteLine($"    State        : {p.State}");
        Console.WriteLine($"    Language     : {p.Language}");
        Console.WriteLine($"    Steam Deck   : {(p.IsRunningOnSteamDeck ? "yes" : "no")}");
    }
}
