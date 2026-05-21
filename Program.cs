using SteamOwnershipExample.Samples;

PrintMenu();

while (true)
{
    Console.Write("Select [1-4 / m / q]: ");
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
        case "1":
        case "2":
        case "3":
        case "4":
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
    Console.WriteLine("=== Steam Ownership Example — Samples ===");
    Console.WriteLine();
    Console.WriteLine("  [1] Basic ownership check (FNAF1)");
    Console.WriteLine("  [2] Multiple games in parallel");
    Console.WriteLine("  [3] Custom IOwnershipProvider impl (Mock)");
    Console.WriteLine("  [4] Steam Web API provider");
    Console.WriteLine("  [m] Show this menu again");
    Console.WriteLine("  [q] Quit");
    Console.WriteLine();
}
