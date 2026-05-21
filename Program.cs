using System.Text;
using SteamOwnershipExample.Samples;

Console.OutputEncoding = Encoding.UTF8;

PrintMenu();

while (true)
{
    Console.Write("Select [1-9 / m / q]: ");
    var choice = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "";
    Console.WriteLine();

    switch (choice)
    {
        case "q":
        case "exit":
            return;
        case "":
            continue;
        case "m":
            PrintMenu();
            continue;
        case "1": case "2": case "3": case "4":
        case "5": case "6": case "7": case "8": case "9":
            await RunSafely(choice);
            break;
        default:
            Console.WriteLine($"  Unknown choice: '{choice}'");
            Console.WriteLine();
            break;
    }
}

static async Task RunSafely(string choice)
{
    try
    {
        Task task = choice switch
        {
            "1" => Sample01_Basic.RunAsync(),
            "2" => Sample02_MultipleGames.RunAsync(),
            "3" => Sample03_CustomProvider.RunAsync(),
            "4" => Sample04_WebApi.RunAsync(),
            "5" => Sample05_PlayerStats.RunAsync(),
            "6" => Sample06_UserInfo.RunAsync(),
            "7" => Sample07_Friends.RunAsync(),
            "8" => Sample08_CloudStorage.RunAsync(),
            "9" => Sample09_Overlay.RunAsync(),
            _   => Task.CompletedTask,
        };
        await task;
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"    ERROR: {ex.Message}");
    }
    finally
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 60));
        Console.WriteLine();
    }
}

static void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine("=== Steam Ownership Example - Samples ===");
    Console.WriteLine();
    Console.WriteLine("  Ownership:");
    Console.WriteLine("    [1] Basic ownership check (FNAF1)");
    Console.WriteLine("    [2] Multiple games in parallel");
    Console.WriteLine("    [3] Custom IOwnershipProvider impl (Mock)");
    Console.WriteLine("    [4] Steam Web API provider");
    Console.WriteLine();
    Console.WriteLine("  Player progress:");
    Console.WriteLine("    [5] Achievements and stats");
    Console.WriteLine();
    Console.WriteLine("  Identity and social:");
    Console.WriteLine("    [6] User info (name, ID, Steam Deck detection)");
    Console.WriteLine("    [7] Friends list");
    Console.WriteLine();
    Console.WriteLine("  Storage:");
    Console.WriteLine("    [8] Steam Cloud (write / list / read / delete)");
    Console.WriteLine();
    Console.WriteLine("  Presentation:");
    Console.WriteLine("    [9] Overlay and Rich Presence");
    Console.WriteLine();
    Console.WriteLine("  [m] Show this menu again");
    Console.WriteLine("  [q] Quit");
    Console.WriteLine();
}
