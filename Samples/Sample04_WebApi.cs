using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample04_WebApi
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[4] Steam Web API provider");
        Console.WriteLine("    REST call — no Steam client needed, but requires an API key and a SteamID64.");
        Console.WriteLine();

        var apiKey = Environment.GetEnvironmentVariable("STEAM_API_KEY");
        var steamIdStr = Environment.GetEnvironmentVariable("STEAM_ID64");

        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(steamIdStr))
        {
            Console.WriteLine("    Skipped: STEAM_API_KEY and/or STEAM_ID64 environment variables are not set.");
            Console.WriteLine();
            Console.WriteLine("    PowerShell:");
            Console.WriteLine("      $env:STEAM_API_KEY = \"<key from https://steamcommunity.com/dev/apikey>\"");
            Console.WriteLine("      $env:STEAM_ID64    = \"<your 17-digit SteamID64>\"");
            return;
        }

        await using IOwnershipProvider provider = new SteamWebApiOwnershipProvider(
            apiKey, ulong.Parse(steamIdStr));

        SampleHelpers.PrintResult(
            await provider.CheckAsync(KnownApps.FiveNightsAtFreddys1));
    }
}
