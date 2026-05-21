using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample01_Basic
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[1] Basic ownership check");
        Console.WriteLine("    Single CheckAsync call for one known game.");
        Console.WriteLine();

        await using IOwnershipProvider provider = new SteamworksOwnershipProvider();

        var result = await provider.CheckAsync(KnownApps.FiveNightsAtFreddys1);

        SampleHelpers.PrintResult(result);
    }
}
