using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample09_Overlay
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[9] Overlay and Rich Presence");
        Console.WriteLine("    Overlay only renders on top of a game window — from a console app");
        Console.WriteLine("    you can still call the APIs and see them succeed.");
        Console.WriteLine();

        await using IOverlayController overlay = new SteamworksOverlayController();

        Console.WriteLine($"    Overlay enabled in client: {(overlay.IsOverlayEnabled ? "yes" : "no")}");
        Console.WriteLine();

        Console.WriteLine("    Setting Rich Presence:");
        await overlay.SetRichPresenceAsync("status", "Browsing the Steam Ownership samples");
        await overlay.SetRichPresenceAsync("steam_display", "#Status_Browsing");
        Console.WriteLine("      status         = Browsing the Steam Ownership samples");
        Console.WriteLine("      steam_display  = #Status_Browsing");
        Console.WriteLine();

        Console.WriteLine("    Triggering a Steam screenshot:");
        await overlay.TriggerScreenshotAsync();
        Console.WriteLine("      TriggerScreenshot called.");
        Console.WriteLine();

        Console.WriteLine("    Other things you can call (not invoked here to avoid surprise pop-ups):");
        Console.WriteLine("      await overlay.ActivateAsync();");
        Console.WriteLine("      await overlay.ActivatePageAsync(OverlayPage.Achievements);");
        Console.WriteLine("      await overlay.ActivateProfileAsync(76561197960287930);");
        Console.WriteLine("      await overlay.ActivateWebPageAsync(\"https://store.steampowered.com\");");

        await overlay.ClearRichPresenceAsync();
    }
}
