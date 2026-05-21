using SteamOwnershipExample.Core;

namespace SteamOwnershipExample.Samples;

internal static class SampleHelpers
{
    public static void PrintResult(OwnershipResult r)
    {
        var status = r.IsOwned ? "OWNED    " : "NOT OWNED";
        Console.WriteLine($"    [{status}]  AppId {r.AppId,-7} via {r.ProviderName}");
        if (r.Notes is not null)
            Console.WriteLine($"                 Note: {r.Notes}");
    }
}
