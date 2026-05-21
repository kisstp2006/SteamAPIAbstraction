using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample02_MultipleGames
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[2] Multiple games in parallel");
        Console.WriteLine("    Task.WhenAll on the interface — provider implementation does not matter.");
        Console.WriteLine();

        AppId[] games =
        [
            KnownApps.FiveNightsAtFreddys1, // 319510
            new AppId(620),                 // Portal 2
            new AppId(440),                 // Team Fortress 2
            new AppId(730),                 // Counter-Strike 2
            new AppId(12345)                // intentionally non-existent
        ];

        await using IOwnershipProvider provider = new SteamworksOwnershipProvider();

        var results = await Task.WhenAll(
            games.Select(id => provider.CheckAsync(id)));

        foreach (var r in results)
            SampleHelpers.PrintResult(r);
    }
}
