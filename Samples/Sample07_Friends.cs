using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample07_Friends
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[7] Friends list");
        Console.WriteLine("    Enumerate Steam friends and show who's playing what right now.");
        Console.WriteLine();

        await using IFriendsProvider friends = new SteamworksFriendsProvider();

        var list = await friends.ListFriendsAsync();
        Console.WriteLine($"    {list.Count} friends total.");
        Console.WriteLine();

        var online = list
            .Where(f => f.State != UserOnlineState.Offline && f.State != UserOnlineState.Invisible)
            .Take(10)
            .ToArray();

        if (online.Length == 0)
        {
            Console.WriteLine("    No online friends right now.");
            return;
        }

        Console.WriteLine($"    First {online.Length} online (max 10):");
        foreach (var f in online)
        {
            var playing = f.CurrentlyPlaying is { } appId
                ? $"  -> playing AppId {appId}"
                : "";
            Console.WriteLine($"      {f.State,-10}  {f.PersonaName}{playing}");
        }
    }
}
