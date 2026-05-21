using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample05_PlayerStats
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[5] Achievements and stats");
        Console.WriteLine("    Stats/achievements are scoped to the running app (steam_appid.txt).");
        Console.WriteLine("    With 480 (Spacewar) we read Spacewar's predefined achievements/stats.");
        Console.WriteLine();

        await using IPlayerStatsProvider stats = new SteamworksPlayerStatsProvider();

        string[] spacewarAchievements =
        [
            "ACH_WIN_ONE_GAME",
            "ACH_WIN_100_GAMES",
            "ACH_TRAVEL_FAR_ACCUM",
            "ACH_TRAVEL_FAR_SINGLE",
            "ACH_HEAVY_FIRE"
        ];

        Console.WriteLine("  Achievements:");
        foreach (var apiName in spacewarAchievements)
        {
            var info = await stats.GetAchievementAsync(apiName);
            var status = info.IsUnlocked ? "UNLOCKED" : "LOCKED  ";
            var when = info.UnlockedAt is { } ts ? $" @ {ts:yyyy-MM-dd HH:mm} UTC" : "";
            Console.WriteLine($"    [{status}] {info.ApiName,-25} {info.DisplayName ?? "(no display name)"}{when}");
        }

        Console.WriteLine();
        Console.WriteLine("  Stats:");
        Console.WriteLine($"    NumGames     = {await stats.GetIntStatAsync("NumGames")}");
        Console.WriteLine($"    NumWins      = {await stats.GetIntStatAsync("NumWins")}");
        Console.WriteLine($"    NumLosses    = {await stats.GetIntStatAsync("NumLosses")}");
        Console.WriteLine($"    FeetTraveled = {await stats.GetFloatStatAsync("FeetTraveled"):F2}");

        Console.WriteLine();
        Console.WriteLine("  Write demo (not committed to avoid mutating your Spacewar stats):");
        Console.WriteLine("    await stats.UnlockAchievementAsync(\"ACH_WIN_ONE_GAME\");");
        Console.WriteLine("    await stats.SetIntStatAsync(\"NumGames\", 42);");
        Console.WriteLine("    await stats.IndicateAchievementProgressAsync(\"ACH_WIN_100_GAMES\", 35, 100);");
        Console.WriteLine("    await stats.CommitAsync();    // flush to Steam");
    }
}
